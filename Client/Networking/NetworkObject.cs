using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Client.Networking;

using Client.Game.Data;
using Client.Networking.Arguments;
using PLoginAccount = Packets.PLoginAccount;
public sealed class NetworkObject : NetState
{
    public bool ShouldRebuild { get; private set; } = true;

    private bool m_IsOpen;
    private Socket? m_Socket;
    private IOStream m_Stream;
    private IPAddress m_Address;
    private byte[] m_RecvBuffer;
    private AsyncCallback m_RecvCallback;
    private AsyncCallback m_SendCallback;
    private ConcurrentQueue<Packet> _packetQueue = new ConcurrentQueue<Packet>();
    private Task? _sendingTask;
    private bool _isSending = false;
    public NetworkObject()
    {
        m_Stream = IOStream.Construct();
        m_Address = IPAddress.None;
        m_RecvBuffer = new byte[2048];
        m_RecvCallback = new AsyncCallback(EndReceive);
        m_SendCallback = new AsyncCallback(EndSend);
    }
    public override bool IsOpen => m_IsOpen;
    public override IOStream Stream => m_Stream;
    public override IPAddress Address => m_Address ?? IPAddress.None;
    public override bool Attach(Socket socket)
    {
        if (socket == null) { return false; }
        var e = new ConnectionEventArgs(socket, this);
        if (e.IsAllowed = socket.Connected)
        {
            var lingerOption = socket.LingerState;
            if (lingerOption != null)
                lingerOption.Enabled = Network.Constants.EnabledLingering;
            
            if (ShouldRebuild)
            {
                m_Stream.Reset();
                ShouldRebuild = false;
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
            e.IsReady = true;
            Receive();
        }
        Network.Attach(e);
        return e.IsAllowed && e.IsReady;
    }
    public override void Detach()
    {
        lock (Network.SharedLock)
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
                if (Ingame)
                    Logger.LogError($"Reason: {reason}");
                
                Detach();
            }
        }
    }
    private void Receive()
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
            if (length <= 0)
                throw new SocketException(errorCode: (int)SocketError.NotConnected);            
            lock (m_Stream.Input)
                m_Stream.Crypto.Decrypt(m_RecvBuffer, 0, length, m_Stream.Input);
            Receive();
        }
        catch (SocketException ex)
        {
            InternalFailure(ex);

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
            if (!m_Socket.Connected)
                throw new SocketException((int)SocketError.ConnectionAborted);
            
            int length = m_Socket.EndSend(asyncResult);
            if (length <= 0)
            {
                Detach();
                return;
            }

            var gram = m_Stream.Output.Proceed();
            if (gram == null)
                return;
            Send(gram);
        }
        catch (Exception exception)
        {
            InternalFailure(exception);
        }
    }
    private void Send(byte[]? buffer)
    {
        if (m_IsOpen && (buffer != null))
        {
            try
            {
                Utility.FormatBuffer(Console.Out, buffer, ConsoleColor.DarkMagenta);
                m_Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, m_SendCallback, null);
            }
            catch (Exception exception)
            {
                InternalFailure(exception);
            }
        }
    }
    public override void Send(Packet? packet)
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
        
        var e = new PLoginAccount.LoginAuthEventArgs(this, account.Username, account.Password);
        try
        {
            Send(PLoginAccount.Instantiate(e));
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
                Logger.Log($"Client -> Server: {packet.GetType().Name} (0x{packet.ID:X2}, {packet.Length}) {(packet.Encode ? "(encoded)" : string.Empty)} {(packet.Fixed ? "(dynamic)" : string.Empty)} ({m_Stream.Crypto})", LogColor.Magenta);
                try
                {
                    byte[] buffer = packet.Compile();
                    if (buffer.Length > 0)
                    {
                        if (packet.Encode)
                        {
                            m_Stream.Crypto.Encrypt(buffer, 0, buffer.Length, m_Stream.Input);
                        }
                        else
                        {
                            m_Stream.Output.Enqueue(buffer, 0, buffer.Length);
                        }
                        await Task.Run(() => Send(m_Stream.Output.Proceed()));
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
                while ((Stream.Input.Count > 0) && IsOpen)
                {
                    var packetID = Stream.Input.GetPacketID(); // (byte)
                    if (packetID < 0)
                        break;   

                    PacketHandler? ph = PacketHandlers.GetHandler(packetID);
                    if (ph == null)
                    {
                        Logger.Log($"Server -> Client: Unhandled packet (0x{packetID:X2}, {m_Stream.Input.GetPacketLength()})", LogColor.Invalid);
                        Span<byte> badBuffer = Stream.Input.Dequeue(Stream.Input.Count).AsSpan();
                        new PacketReader(badBuffer, false, (byte)packetID, "Unknown").Trace(true);
                        break;
                    }

                    int length = ph.Length <= 0 ? Stream.Input.GetPacketLength() : ph.Length;
                    var ns = Network.State;
                    var canRead = length > 0 && (ns != null) && (ns.ConfirmedLogin && length >= 1 || ns.Ingame && length >= 2);
                    Logger.Log($"Server -> Client: {ph.Name} (0x{packetID:X2}, {ph.Length}).. length:{length} ({(canRead ? "in" : "out-of")})-game", LogColor.Info);
                    if (length < 3 && !canRead)
                    {
                        Logger.PushWarning("Detaching after receiving bad packet length!");
                        break;
                    }
                    Span<byte> buffer = Stream.Input.Dequeue(length);
                    Utility.FormatBuffer(Console.Out, buffer.ToArray(), color: ConsoleColor.DarkGray);
                    PacketReader reader = PacketReader.Create(ref buffer, ref ph);
                    ph.Receive(this, reader);
                    var line = $"Sliced network event ({length} byte{(length == 1 ? string.Empty : "s")})\n";
                    Logger.Log(line, color: IsOpen ? LogColor.Success : LogColor.Invalid);
                }
            }
        }
    }

    public override string ToString() => m_Address == null ? "unknown" : m_Address.ToString();
}
