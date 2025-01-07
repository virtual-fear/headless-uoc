namespace Client.Networking
{
    using System.Net;
    using System.Net.Sockets;
    using Arguments;
    using Client.Accounting;
    using Client.Networking.Outgoing;

    public enum ConnectionAck
    {
        FirstLogin,
        SecondLogin,
    }
    public struct ConnectInfo
    {
        public ConnectionAck Stage;
        public ConnectInfo() => Stage = ConnectionAck.FirstLogin;
        public IPEndPoint EndPoint = new IPEndPoint(IPAddress.None, 0);
        public String? Username = string.Empty;
        public String? Password = string.Empty;
        public UInt32 Seed = 0;
    }

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

        public static readonly object SharedLock = new object();

        /// <summary>
        ///    Network information about the current connection.
        /// </summary>
        internal static ConnectInfo Info { get; set; }
        public static Socket? Socket { get; private set; }
        public static NetState? State { get; protected set; } 
        public static bool IsAttached => State?.IsOpen ?? false;
        internal static async void AsyncConnect()
        {
            String processName = Application.ProcessName;
            IPEndPoint serverEP = Network.Info.EndPoint;
            if (Socket == null || Socket.Blocking)
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult asyncResult = Socket.BeginConnect(serverEP, null, null);
            Logger.Log(processName, $"Establishing connection with {serverEP}", LogColor.Info);
            try
            {
                Socket.EndConnect(asyncResult);
                Logger.Log(processName, $"Connected to {serverEP.Address}", LogColor.Success);
                Network.Connect(new SocketEventArgs(Socket));
                if ((State != null) && State.Attach(Socket))
                {
                    Logger.Log($"{State.Address}: Network cycle attached.");
                    await Task.Run(delegate () { while ((State != null) && State.IsOpen) State.Slice(); });
                }
                else
                {
                    Logger.LogError($"{serverEP.Address}: Failed to attach socket to network state.");
                }
            }
            catch (SocketException socketError)
            {
                Logger.LogError(what: $"({nameof(SocketException)})\n{socketError.Message}");
            }
            catch (Exception error)
            {
                Logger.LogError(what: error.Message);
            }
            await Task.CompletedTask;
        }
        static Network()
        {
            OnConnect += Network_OnConnect;
            OnDisconnect += Network_OnDisconnect;
            OnConstruct += Network_OnConstruct;
            OnAttach += Network_OnAttach;
            OnDetach += Network_OnDetach;
        }

        private static void Network_OnDetach(NetState ns)
        {
            Logger.PushWarning($"{ns.Address}: Detached network state");

            // Reconnect with the seed
            Task.Run(AsyncConnect);
        }
        private static void Network_OnAttach(ConnectionEventArgs e)
        {
            Logger.Log(Application.ProcessName, $"{e.State.Address} attached to network state.", LogColor.Info);
            ConnectInfo info = Network.Info;
            string username = info.Username ?? string.Empty;
            string password = info.Password ?? string.Empty;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Logger.LogError("Username or password is empty.");
                return;
            }
            if (info.Stage == ConnectionAck.FirstLogin)
            {
                if (State == null)
                    throw new ArgumentNullException(nameof(State));

                PInitialSeed.SendBy(State);
                State.Slice();
                // TODO: Build an account event management system
                State.Login(new Account(username, password));
                e.IsAllowed = true;
            }
        }

        private static void Network_OnConstruct(NetState ns) => Logger.Log(Application.ProcessName, "Constructed network state.", LogColor.Info);
        private static void Network_OnDisconnect(SocketEventArgs e) => Console.WriteLine(Application.ProcessName, $"{e.Address} disconnected from the server.");
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
