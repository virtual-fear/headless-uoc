namespace Client.Networking.Arguments
{
    using System;
    using System.Net.Sockets;
    public sealed class ConnectionEventArgs : EventArgs
    {
        public Socket Socket { get; }
        public NetState State { get; }
        internal ConnectionEventArgs(Socket socket, NetState state)
        {
            Socket = socket;
            State = state;
        }
        public bool IsAllowed { get; set; } = false;
        public bool IsReady { get; set; } = false;
        public bool Rebuild { get; set; } = false;
    }

}
