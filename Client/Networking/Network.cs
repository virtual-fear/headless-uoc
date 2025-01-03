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

    public enum ConnectionAck
    {
        FirstLogin,
        SecondLogin,
    }

    public struct ConnectInfo
    {
        public ConnectionAck Stage;
        public ConnectInfo() => Stage = ConnectionAck.FirstLogin;
        public IPEndPoint? EndPoint;
        public String? Username;
        public String? Password;
        public Int32 Seed;
    }

    /// <summary>
    ///   Using this partial class provides us with the flexibility to expose events without explicitly showing our invocation methods. 
    /// <para><see cref="NetworkObject"/> inherits the abstract <see cref="NetState"/> class to handle the network internally.</para>
    /// </summary>
    public partial class Network
    {
        public static readonly object SharedLock = new object();

        /// <summary>
        ///    Network information about the current connection.
        /// </summary>
        public static ConnectInfo? Info { get; protected set; }
        public static Socket Socket { get; }
        public static NetState State { get; }
        public static bool IsAttached => State?.IsOpen ?? false;
        public static bool IsConfigured { get; private set; } = false;
        public static bool IsReconnecting { get; private set; } = false;
        protected static async Task _Main() => await Task.Delay(-1); // (wait indefinitely)
        static Network()
        {
            // TODO: Reach out to the user to get the username (or use config file)
            // Maybe implement string[] args to get the username and password
            Info = new ConnectInfo()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2593),
                Username = "admin",
                Password = "admin",
                Seed = 1
            };
            PacketHandlers.Configure();
            Network.OnConnect += Network_OnConnect;
            Network.OnDisconnect += Network_OnDisconnect;
            Network.OnConstruct += Network_OnConstruct;
            Network.OnAttach += Network_OnAttach;
            Network.OnDetach += Network_OnDetach;

            // TODO: Create a class to handle the Configuration setup of PacketSink
            // so when the app is run, it can determine what events need to be used
            PacketSink.ServerList += ReceivedServerList_0xA8;
            PacketSink.ServerAck += ReceivedServerAck_0x8C;

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            State = new NetworkObject();
        }

        private static async void ReceivedServerAck_0x8C(ServerAckEventArgs e)
        {
            PacketSink.ServerAck -= ReceivedServerAck_0x8C;
            Logger.Log(typeof(Assistant), "Received acknowledgement from the server.", ConsoleColor.Magenta);
            Logger.Log($"{new string(' ', nameof(Assistant).Length + -4)}Seed:0x{e.Seed:X4}", ConsoleColor.DarkGreen);
            var v = Info.Value;
            v.Seed = e.Seed;
            v.Stage = ConnectionAck.SecondLogin;
            Info = v;
            Socket?.Disconnect(reuseSocket: true);
            if (Socket == null || Socket.Blocking)
                Assistant.Main();
        }

        //[Obsolete("This event method is not used in the original code (used only for testing purposes)", error: false)]
        private static void ReceivedServerList_0xA8(ServerListReceivedEventArgs e)
        {
            PacketSink.ServerList -= ReceivedServerList_0xA8;
            var shard = e.ServerListEntries.FirstOrDefault();
            if (shard == null)
            {
                Logger.Log("No shards are currently available", ConsoleColor.Red);
                return;
            }
            Logger.Log("  > Connecting to the first shard available!", ConsoleColor.Yellow);
            Network.State.Send(PPlayServer.Instantiate((byte)shard.Index));
        }

        private static void Network_OnDetach(NetState e)
        {
            Network.OnDetach -= Network_OnDetach;
            Logger.Log("Network detached state", ConsoleColor.Yellow);

            // Reconnect with the seed
            Task.Run(Assistant.Main);
        }

        private static void Network_OnAttach(ConnectionEventArgs e)
        {
            if (Info.HasValue)
            {
                Logger.Log($"{e.State.Address}: Attached to the network..", ConsoleColor.Magenta);
                ConnectInfo info = Info.Value;
                string username = info.Username ?? string.Empty;
                string password = info.Password ?? string.Empty;
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Logger.LogError("Username or password is empty.");
                    return;
                }
                switch (info.Stage)
                {
                    case ConnectionAck.FirstLogin:
                        PSeed.SendBy(State, State.Address.ToUInt32());
                        State.Login(new Account(info.Username, info.Password));
                        State.Send(LoginAuth.Instantiate(new LoginAuthEventArgs(State, info.Username, info.Password)));
                        e.IsAllowed = true;
                        break;
                    case ConnectionAck.SecondLogin:
                        Logger.Log($"{e.State.Address}: Sending ack response.", ConsoleColor.Magenta);
                        Int32 v = info.Seed;
                        Span<byte> b = stackalloc byte[4] {
                            (byte)(v >> 0x18),	// 24
										(byte)(v >> 0x10),	// 16 
										(byte)(v >> 0x08),	//  8
										(byte)(v >> 0x00)	// 00
                        };
                        Socket.Send(b);
                        State.Crypto = new GameCrypto();
                        State.Send(SecondLoginAuth.Instantiate(new SecondLoginAuthEventArgs(State, info.Username, info.Password, info.Seed)));
                        break;
                }
            } else
            {
                e.IsAllowed = false;
            }
        }

        private static async void Network_OnConstruct(NetState e)
        {
            IPEndPoint serverEP = Info.Value.EndPoint ?? new IPEndPoint(IPAddress.None, -1);
            if (serverEP.Address == IPAddress.None || serverEP.Port == -1)
            {
                Logger.LogError("Server endpoint is invalid.");
                return;
            }
            Logger.Log($"{serverEP.Address}: Network state constructed", ConsoleColor.White);
            IAsyncResult asyncResult = Socket.BeginConnect(serverEP, null, null);
            String remoteAddress = serverEP.Address.ToString();
            Logger.Log(remoteAddress, "Establishing connection...");
            try
            {
                Socket.EndConnect(asyncResult);
                Logger.Log(remoteAddress, "Connection established.", ConsoleColor.Green);
                Network.Connect(new SocketEventArgs(Socket));
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

        private static void Network_OnDisconnect(SocketEventArgs e)
        {
            Console.WriteLine($"{e.Address}: Disconnected from the server.");
        }
        private static async void Network_OnConnect(SocketEventArgs e)
        {
            Console.WriteLine($"{e.Address}: Connected to the server.");
            if (State.Attach(e.Socket))
            {
                Logger.Log(e.Address, "Running network cycle", ConsoleColor.White);
                await Task.Run(delegate () { while (Socket.Connected && State.IsOpen) State.Slice(); });
            }
            else
            {
                throw new SocketException((int)SocketError.ConnectionRefused);
            }
        }
    }

}
