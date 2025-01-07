using System.Net.Sockets;

namespace Client.Networking
{
    using Client.IO;
    using Cryptography;
    public abstract class Receiver
    {
        public AsyncCallback Callback { get; }
        protected Receiver() => Callback = new(EndReceive);
        protected abstract void Disconnected();
        protected abstract byte[] GetBuffer();
        protected abstract Crypto GetCrypto();
        protected abstract BaseQueue GetInput();
        protected abstract Socket GetSocket();
        protected abstract void Trace(Exception exception);
        private void BeginReceive()
        {
            lock (this)
            {
                Socket socket = this.GetSocket();
                if ((socket != null) && socket.Connected)
                {
                    byte[] buffer = GetBuffer();
                    try
                    {
                        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, Callback, null); 
                    }
                    catch (Exception exception)
                    {
                        Trace(exception);
                    }
                }
            }
        }

        private void EndReceive(IAsyncResult asyncResult)
        {
            try
            {
                int length = GetSocket().EndReceive(asyncResult);
                if (length > 0)
                {
                    byte[] buffer = GetBuffer();
                    Crypto crypto = GetCrypto();
                    BaseQueue input = GetInput();
                    lock (input) { crypto.Decrypt(buffer, 0, length, input); }
                    BeginReceive();
                }
                else
                {
                    throw new SocketException(errorCode: (int)SocketError.NotConnected);
                }
            }
            catch (SocketException)
            {
                Disconnected();
            }
            catch (Exception exception)
            {
                Trace(exception);
            }
        }
    }
}
