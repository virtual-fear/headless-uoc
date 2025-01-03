namespace Client.Networking
{
    using System.Net;
    using System.Net.Sockets;
    using Arguments;
    using Client.Accounting;
    using Client.Cryptography.Impl;
    using Client.Networking.Incoming;
    using Client.Networking.Outgoing;
    using static Client.Networking.Incoming.PacketSink;
    using static Client.Networking.Outgoing.LoginAuth;
    using static Client.Networking.Outgoing.SecondLoginAuth;

    public enum ConnectionAck
    {
        FirstLogin,
        SecondLogin,
    }
    public struct ConnectInfo
    {
        public ConnectionAck Stage;
        public ConnectInfo() => Stage = ConnectionAck.FirstLogin;
        public IPEndPoint? EndPoint = new IPEndPoint(IPAddress.None, -1);
        public String? Username = string.Empty;
        public String? Password = string.Empty;
        public Int32 Seed = 0;
    }

    /// <summary>
    ///   Using this partial class provides us with the flexibility to expose events without explicitly showing our invocation methods. 
    /// <para><see cref="NetworkObject"/> inherits the abstract <see cref="NetState"/> class to handle the network internally.</para>
    /// <para><see cref="Network"/> events should be handled in the <see cref="Assistant"/> class.</para>
    /// </summary>
    public partial class Network
    {
        public static readonly object SharedLock = new object();

        /// <summary>
        ///    Network information about the current connection.
        /// </summary>
        public static ConnectInfo? Info { get; protected set; }
        public static Socket? Socket { get; protected set; }
        public static NetState? State { get; protected set; }
        public static bool IsAttached => State?.IsOpen ?? false;
        public static bool IsConfigured { get; private set; } = false;
        public static bool IsReconnecting { get; private set; } = false;
    }

    /**
     *  Network events are handled here.
     * */
    public delegate void NetworkEventHandler<T>(T e);
    public partial class Network
    {
        /// <summary>
        ///     <see cref="Socket"/> has connected to the server
        /// </summary>
        internal static event NetworkEventHandler<SocketEventArgs>? OnConnect;

        /// <summary>
        ///     <see cref="Socket"/> has disconnected from the server
        /// </summary>
        internal static event NetworkEventHandler<SocketEventArgs>? OnDisconnect;

        /// <summary>
        ///     <see cref="NetState"/> gets constructed
        /// </summary>
        internal static event NetworkEventHandler<NetState>? OnConstruct;

        /// <summary>
        ///     A connection has been established with the server using <see cref="NetState"/> and is currently being attached to the network.
        /// </summary>
        internal static event NetworkEventHandler<ConnectionEventArgs>? OnAttach;

        /// <summary>
        ///     Server was shutdown or failed to connect, <see cref="NetState"/> failed to maintain the connection.
        /// </summary>
        internal static event NetworkEventHandler<NetState>? OnDetach;

        #region Methods

        /// <summary>
        ///     When the client socket has connected to the server.
        /// </summary>
        protected static void Connect(SocketEventArgs e) => OnConnect?.Invoke(e);

        /// <summary>
        ///    When the client socket has disconnected from the server.
        /// </summary>
        /// <param name="e"></param>
        protected static void Disconnect(SocketEventArgs e) => OnDisconnect?.Invoke(e);

        /// <summary>
        ///     When the <see cref="NetState"/> has been constructed.
        /// </summary>
        /// <param name="ns"></param>
        protected static void Construct(NetState ns) => OnConstruct?.Invoke(ns);

        /// <summary>
        ///     When the client socket has established a connection with the server and is attached to the network.
        /// </summary>
        protected static void Attach(ConnectionEventArgs c) => OnAttach?.Invoke(c);

        /// <summary>
        ///     When the state has failed to maintain a connection for the following reasons.
        ///     <para>1) Server could be offline</para>    
        ///     <para>2) Server could have rejected the connection</para>
        ///     <para>3) You may be offline</para>
        /// </summary>
        protected static void Detach(NetState state) => OnDetach?.Invoke(state);

        #endregion
    }

}
