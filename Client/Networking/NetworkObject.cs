using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace Client.Networking
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Accounting;
    using Arguments;
    using Cryptography;
    using IO;
    using LoginAuth = Outgoing.LoginAuth;
    public sealed class NetworkObject : NetState
    {
        public bool Rebuild { get; private set; } = true;

        private bool m_IsOpen;
        private Socket m_Socket;
        private IPAddress m_Address;
        private byte[] m_RecvBuffer;
        private AsyncCallback m_RecvCallback;
        private AsyncCallback m_SendCallback;
        private BaseQueue m_Input = new InputQueue();
        private BaseQueue m_Output = new OutputQueue();
        private Crypto m_Crypto;

        private ConcurrentQueue<Packet> _packetQueue = new ConcurrentQueue<Packet>();
        private Task _sendingTask;
        private bool _isSending;
        public override bool IsOpen => m_IsOpen;
        public override Crypto Crypto { get; set; }
        public override IPAddress Address => m_Address;
        public override BaseQueue Input => m_Input;
        public override bool Attach(Socket socket)
        {
            if (socket == null) { return false; }
            var connection = new ConnectionEventArgs(socket, this);
            if (connection.IsAllowed = socket.Connected)
            {
                var lingerOption = socket.LingerState;
                if (lingerOption != null)
                    lingerOption.Enabled = false;
                
                if (Rebuild)
                {
                    m_RecvBuffer = new byte[2048];
                    m_RecvCallback = new AsyncCallback(EndReceive);
                    m_SendCallback = new AsyncCallback(EndSend);
                    m_Crypto = Crypto.Instantiate();
                    m_Output.Clear();
                    m_Input.Clear();
                    Rebuild = false;
                }
                m_IsOpen = true;
                m_Socket = socket; 
                if (socket.RemoteEndPoint is IPEndPoint remoteEP)
                {
                    m_Address = remoteEP.Address;
                }
                else
                {
                    throw new InvalidOperationException("Socket RemoteEndPoint is not an IPEndPoint.");
                }
                connection.IsReady = true;
                Receive();
                Network.Attach(connection);
            }
            return connection.IsAllowed && connection.IsReady;
        }
        public override void Detach()
        {
            if (m_IsOpen)
            {
                m_IsOpen = false;
                try
                {
                    m_Socket.Shutdown(SocketShutdown.Both);
                }
                catch { }
                try
                {
                    m_Socket.Close();
                }
                catch { }
                Network.Detach(this);
            }
        }
        private void Disconnected()
        {
            Logger.Log("Disconnected.");
            Detach();
        }
        private void InternalFailure(Exception exception)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(exception.ToString());
            string e = message.ToString();
            message.Clear();
            InternalFailure(e);
        }
        private void InternalFailure(string reason)
        {
            lock (this)
            {
                if (m_IsOpen)
                {
                    Logger.LogError("Internal network has stopped working..");
                    Logger.Log($"\tReason: {reason}\n");
                    Detach();
                }
            }
        }
        private void    Receive()
        {
            lock (Network.SharedLock)
            {
                if (m_IsOpen)
                {
                    try
                    {
                        m_Socket.BeginReceive(m_RecvBuffer, 0, m_RecvBuffer.Length, SocketFlags.None, m_RecvCallback, null);
                    }
                    catch (Exception exception)
                    {
                        InternalFailure(exception);
                    }
                }
            }
        }
        private void EndReceive(IAsyncResult asyncResult)
        {
            try
            {
                if (m_Socket == null) { return; }
                int length = m_Socket.EndReceive(asyncResult);
                if (length <= 0) { return; }
                lock (m_Crypto)
                {
                    m_Crypto.Decrypt(m_RecvBuffer, 0, length, m_Input);
                }
                Receive();
            }
            catch (SocketException ex)
            {
                Logger.LogError(ex.Message);
                Disconnected();
            }
            catch (Exception exception)
            {
                InternalFailure(exception);
            }
        }
        private void EndSend(IAsyncResult asyncResult)
        {
            try
            {
                int length = m_Socket.EndSend(asyncResult);
                if (length <= 0)
                {
                    Disconnected();
                    return;
                }
                OutputQueue output = (OutputQueue)m_Output;
                var gram = output.Proceed();
                if (gram == null)
                {
                    return;
                }
                Send(gram);
            }
            catch (Exception exception)
            {
                InternalFailure(exception);
            }
        }
        private void Send(byte[] buffer)
        {
            if (m_IsOpen && (buffer != null))
            {
                try
                {
                    m_Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, m_SendCallback, null);
                }
                catch (Exception exception)
                {
                    InternalFailure(exception);
                }
            }
        }
        public override void Send(Packet packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            if (m_IsOpen)
            {
                _packetQueue.Enqueue(packet);
                StartSendingTask();
            }
        }
        private void StartSendingTask()
        {
            if (_isSending)
                return;

            _isSending = true;
            _sendingTask = Task.Run(async () => await ProcessQueue());
        }
        public override bool Login(IAccount account)
        {
            if (account == null) 
                throw new ArgumentNullException("account", "Invalid account login.");
            
            var e = new LoginAuth.LoginAuthEventArgs(this, account.Username, account.Password);
            try
            {
                Send(LoginAuth.Instantiate(e));
                Account = account;
                return true;
            }
            catch { }
            return false;
        }
        private async Task ProcessQueue()
        {
            if (_packetQueue.Count > 0)
            {
                while (_packetQueue.TryDequeue(out Packet? packet))
                {
                    Logger.Log($"Client -> Server: {packet.GetType().Name} (0x{packet.ID:X2}) {(packet.Encode ? "(encoded)" : string.Empty)} {(packet.Fixed ? "(dynamic)" : string.Empty)}", color: ConsoleColor.Green);
                    try
                    {
                        byte[] buffer = packet.Compile();
                        if (buffer.Length > 0)
                        {
                            // GetDiagnostic().Sent(packet, buffer, 0, buffer.Length);
                            if (packet.Encode)
                            {
                                m_Crypto.Encrypt(buffer, 0, buffer.Length, m_Output);
                            }
                            else
                            {
                                m_Output.Enqueue(buffer, 0, buffer.Length);
                            }
                            await Task.Run(delegate
                            {
                                if (m_Output != null)
                                {
                                    Send(((OutputQueue)m_Output).Query());
                                }
                            });
                        }
                    }
                    catch (Exception exception)
                    {
                        InternalFailure(exception);
                    }
                }
            }
            _isSending = false;
        }
        public override void Slice()
        {
            if (m_IsOpen)
            {
                lock (Network.SharedLock)
                {
                    InputQueue ip = (InputQueue)Input;
                    while ((ip.Count > 0) && IsOpen)
                    {
                        int packetID = ip.GetPacketID();
                        if (packetID < 0)
                            break;

                        PacketHandler handler = PacketHandlers.GetHandler(packetID);
                        if (handler == null)
                        {
                            Logger.Log($"Server -> Client: Unhandled packed (0x{packetID:X2}, {ip.GetPacketLength()})", color: ConsoleColor.Red);
                            PacketReader.Initialize(ip.Dequeue(ip.Count), false, (byte)packetID, "Unknown").Trace(true);
                            break;
                        }

                        int length = handler.Length <= 0 ? ip.GetPacketLength() : handler.Length;
                        Logger.Log($"Server -> Client: {handler.Name} (0x{packetID:X2}, {handler.Length}).. length:{length} (done)", ConsoleColor.DarkCyan);
                        if (length < 3)
                        {
                            Logger.PushWarning("Detaching after receiving bad packet length!");
                            Detach();
                            break;
                        }
                        handler.Receive(this, PacketReader.Initialize(ip.Dequeue(length), handler));
                        var line = $"Completed slice with {length} bytes...({(IsOpen ? "still open" : "now closed")})\n";
                        Logger.Log(line, color: IsOpen ? ConsoleColor.DarkYellow : ConsoleColor.DarkRed);
                    }
                }
            }
        }

        public override string ToString() => m_Address == null ? "unknown" : m_Address.ToString();
    }
}
