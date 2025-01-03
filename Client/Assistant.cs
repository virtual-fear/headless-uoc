namespace Client
{
    using System.Net;
    using System.Net.Sockets;
    using Client.Accounting;
    using Client.Cryptography.Impl;
    using Client.Networking;
    using Client.Networking.Arguments;
    using Client.Networking.Incoming;
    using Client.Networking.Outgoing;
    using static Client.Networking.Incoming.PacketSink;
    using static Client.Networking.Outgoing.LoginAuth;
    using static Client.Networking.Outgoing.SecondLoginAuth;

    /// <summary>
    ///     <c>An event driven network client, built for ModernUO.
    ///     <para>Currently supporting .NET Framework v9.0</para>
    ///     </c>
    ///     <para>The main entry point for the Godot engine to internally connect to the remote server.</para>
    ///     <para>It is recommended to use this class to handle and maintain the network events.</para>
    ///     <para>Game events that are network related can be subscribed outside of this class to 
    ///     interact parallel with external game objects.</para>
    /// </summary>
    public partial class Assistant : Network
    {
        // When the State is constructed, the network is attached to the socket.
        // When the network is attached, the network cycle is run.
        // When the network cycle is run, the network is sliced.
        
        [STAThread]
        public static void Main() => WaitIndefinitely().GetAwaiter().GetResult();

        /// <summary>
        ///     The main entry point for the network assistant. 
        ///     <para>This method initiates the asynchronous processing of the application and waits indefinitely for tasks to complete.</para>
        /// </summary>
        /// <returns>A <see cref="Task"</see>> representing the asynchronous operation.</returns>
        protected static async Task WaitIndefinitely() => await Task.Delay(-1);
        static Assistant()
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

            await Task.CompletedTask;
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
            }
            else
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
