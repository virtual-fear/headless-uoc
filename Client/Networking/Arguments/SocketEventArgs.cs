namespace Client.Networking.Arguments
{
    using System.Net;
    using System.Net.Sockets;

    public class SocketEventArgs : EventArgs
    {
        public Socket Socket { get; }
        public SocketEventArgs(Socket socket) => IsBlocked = !(Socket = socket).Blocking;
        public IPAddress Address => (Socket.RemoteEndPoint as IPEndPoint)?.Address ?? IPAddress.None;
        public bool IsConnected => Socket.Connected;
        public bool IsBlocked { get; set; }
    }
}
