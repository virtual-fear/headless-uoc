namespace Client.Networking
{
    using System.Net.Sockets;
    using Arguments;

    public delegate void NetworkEventHandler<T>(T e);

    /// <summary>
    ///   Using this partial class provides us with the flexibility to expose events without explicitly showing our invocation methods. 
    /// <para><see cref="Network"/>.Construct() creates an abstract <see cref="NetState"/> using <see cref="NetworkObject"/> to handle the network internally.</para>
    /// </summary>
    public partial class Network
    {
        public static readonly object SharedLock = new object();
        public static Socket Socket { get; private set; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static NetState State { get; } = NetState.Construct();

        /// <summary>
        ///     Construct an abstract network object
        /// </summary>
        /// <returns></returns>
        protected static NetState Construct() => new NetworkObject();
        protected Network() { if (this is NetState ns) OnConstruct?.Invoke(ns); } // heh

        #region Events

        /// <summary>
        ///     <see cref="NetState"/> gets constructed
        /// </summary>
        internal static event NetworkEventHandler<NetState>? OnConstruct;

        /// <summary>
        ///     <see cref="Socket"/> has connected to the server
        /// </summary>
        internal static event NetworkEventHandler<SocketEventArgs>? OnConnect;

        /// <summary>
        ///     <see cref="Socket"/> has disconnected from the server
        /// </summary>
        internal static event NetworkEventHandler<SocketEventArgs>? OnDisconnect;

        /// <summary>
        ///     A connection has been established with the server using <see cref="NetState"/> and is currently being attached to the network.
        /// </summary>
        internal static event NetworkEventHandler<ConnectionEventArgs>? OnAttach;

        /// <summary>
        ///     Server was shutdown or failed to connect, <see cref="NetState"/> failed to maintain the connection.
        /// </summary>
        internal static event NetworkEventHandler<NetState>? OnDetach;

        #endregion Events

        #region Methods (protected)

        protected static void Connect(SocketEventArgs e) => OnConnect?.Invoke(e);
        protected static void Disconnect(SocketEventArgs e) => OnDisconnect?.Invoke(e);

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

        #endregion Methods (protected)

        /// <summary>
        ///     Last connection to get attached: <see cref="NetState"/>
        /// </summary>
        internal static ConnectionEventArgs? LastConnected { get; set; }
        public static ConnectInfo Info { get; protected set; }
        static Network() => OnAttach += OnAttach_LastConnected;
        private static void OnAttach_LastConnected(ConnectionEventArgs c) => LastConnected = c;
    }
}
