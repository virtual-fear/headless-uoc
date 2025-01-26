namespace Client.Networking
{
    using System.Net;
    using System.Net.Sockets;
    using Arguments;
    using Client.Networking.Data;

    /// <summary>
    ///   Using this partial class provides us with the flexibility to expose events without explicitly showing our invocation methods. 
    /// <para><see cref="NetworkObject"/> inherits the abstract <see cref="NetState"/> class to handle the network internally.</para>
    /// <para><see cref="Network"/> events should be handled in the <see cref="Assistant"/> class.</para>
    /// </summary>
    public partial class Network
    {
        public partial class Constants
        {
            /// <summary>
            ///     Determines if the socket should be lingering after the <see cref="System.Net.Sockets.Socket.Close()"/> is called.
            /// </summary>
            public const bool EnabledLingering = false;
        }

        private static readonly object ConnectLock = new object();
        internal static readonly object SharedLock = new object();
        internal static readonly IPAddress ClientIP = Utility.GetIPAddress();

        /// <summary>
        ///    Network information about the current connection.
        /// </summary>
        public static ConnectInfo Info { get; set; }
        public static Socket? Socket { get; private set; }
        public static NetState? State { get; protected set; }
        public static bool IsAttached => State?.IsOpen ?? false;
        public static async Task<bool> AsyncConnect(string textAddress, int port, string un, string pw)
        {
            var info = Network.Info;
            var addr = IPAddress.Parse(textAddress);
            info.EndPoint = new IPEndPoint(addr, port);
            info.Username = un;
            info.Password = pw;
            info.Seed = 1;
            Network.Info = info;
            return await AsyncConnect();
        }
        public static async Task<bool> AsyncConnect()
        {
            IPEndPoint serverEP = Network.Info.EndPoint;
            if (Socket == null || Socket.Blocking)
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult asyncResult = Socket.BeginConnect(serverEP, null, null);
            try
            {
                Socket.EndConnect(asyncResult);
                Logger.Log(serverEP.Address, $"Connection established.", LogColor.Success);
                Network.Connect(new SocketEventArgs(Socket));
                if ((State != null) && State.Attach(Socket))
                {
                    Logger.Log(State.Address, $"Successfully attached to the network!", LogColor.Info);
                    await Task.Run(delegate ()
                    {
                        lock (Network.ConnectLock)
                        {
                            while ((State != null) && State.IsOpen)
                                State.Slice();
                        }
                    });//.GetAwaiter().GetResult();
                }
                else
                {
                    Logger.LogError($"{serverEP.Address}: Failed to establish a connection with the server.");
                }
            }
            catch (SocketException socketError)
            {
                Logger.LogError(what: $"({nameof(SocketException)})\n{socketError.Message}");
            }
            catch (Exception error)
            {
                Logger.LogError(what: error.Message);
                State?.Detach();
            }
            return await Task.FromResult(State?.IsOpen ?? false);
        }
        static Network()
        {
            OnConnect += Network_OnConnect;
            OnDisconnect += Network_OnDisconnect;
            OnConstruct += Network_OnConstruct;
            OnDetach += Network_OnDetach;
        }

        private static void Network_OnDetach(NetState ns)
        {
            OnDetach -= Network_OnDetach;
            Logger.Log($"{ns.Address}: Detached network state");

            // Reconnect with the seed
            Task.Run(AsyncConnect);
        }
        private static void Network_OnConstruct(NetState ns) => Logger.Log(Application.Name, "Constructed network state.", LogColor.Info);
        private static void Network_OnDisconnect(SocketEventArgs e) => Logger.Log(Application.Name, $"{e.Address} disconnected from the server.");
        private static void Network_OnConnect(SocketEventArgs e) => State ??= new NetworkObject();
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
        public static event NetworkEventHandler<SocketEventArgs>? OnConnect;

        /// <summary>
        ///     <see cref="Socket"/> has disconnected from the server
        /// </summary>
        public static event NetworkEventHandler<SocketEventArgs>? OnDisconnect;

        /// <summary>
        ///     <see cref="NetState"/> gets constructed
        /// </summary>
        public static event NetworkEventHandler<NetState>? OnConstruct;

        /// <summary>
        ///     A connection has been established with the server using <see cref="NetState"/> and is currently being attached to the network.
        /// </summary>
        public static event NetworkEventHandler<ConnectionEventArgs>? OnAttach;

        /// <summary>
        ///     Server was shutdown or failed to connect, <see cref="NetState"/> failed to maintain the connection.
        /// </summary>
        public static event NetworkEventHandler<NetState>? OnDetach;

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
