namespace Client.Networking.Arguments
{
    using System;
    using System.Net.Sockets;
    public class NetworkEventArgs : EventArgs
    {
        protected ConnectionEventArgs Connection { get; }
        public NetworkEventArgs(ConnectionEventArgs c) => Connection = c;
        public Socket Socket => Connection.Socket;
        public NetState Thread => Connection.State;
        public bool IsEnabled => Connection.IsAllowed && Connection.IsReady;
    }
}
