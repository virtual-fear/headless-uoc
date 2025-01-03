using System.Net;
using System.Net.Sockets;

namespace Client
{
    using Accounting;
    using Cryptography.Impl;
    using Networking;
    using Networking.Arguments;
    using Networking.Incoming;
    using Networking.Outgoing;

    using static Networking.Incoming.PacketSink;
    using SecondLoginAuth = Networking.Outgoing.SecondLoginAuth;
    using SecondLoginAuthEventArgs = Networking.Outgoing.SecondLoginAuth.SecondLoginAuthEventArgs;

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

    internal sealed class Assistant : Network
    {
        public static bool IsAttached => State?.IsOpen ?? false;
        public static bool IsConfigured { get; private set; } = false;
        public static bool IsReconnecting { get; private set; } = false;

        [STAThread]
        static async Task Main()
        {
            var serverEP = Assistant.Info.EndPoint;
            if (serverEP == null)
            {
                Logger.LogError("Server is not configured.");
                return;
            }
            IAsyncResult asyncResult = Socket.BeginConnect(serverEP, null, null);
            String remoteAddress = serverEP.Address.ToString();
            Logger.Log(remoteAddress, "Establishing connection...");
            try
            {
                Socket.EndConnect(asyncResult);
                Logger.Log(remoteAddress, "Connection established.", ConsoleColor.Green);
                if (State.Attach(Socket))
                {
                    Logger.Log(remoteAddress, "Running network cycle", ConsoleColor.White);
                    await Task.Run(delegate () { while(Socket.Connected && State.IsOpen) State.Slice(); });
                    await Task.CompletedTask;
                }
                else
                {
                    throw new SocketException((int)SocketError.ConnectionRefused);
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


        private Assistant() { }
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

            Network.OnAttach += Network_OnAttach;
            // TODO: Create a class to handle the Configuration setup of PacketSink
            // so when the app is run, it can determine what events need to be used
            PacketSink.ServerList += ReceivedServerList_0xA8;
            PacketSink.ServerAck += ReceivedServerAck_0x8C;
        }

        private static async void ReceivedServerAck_0x8C(ServerAckEventArgs e)
        {
            PacketSink.ServerAck -= ReceivedServerAck_0x8C;
            Logger.Log(typeof(Assistant), "Received acknowledgement from the server.", ConsoleColor.Magenta);
            Logger.Log($"{new string(' ', nameof(Assistant).Length + -4)}Seed:0x{e.Seed:X4}", ConsoleColor.DarkGreen);
            var v = Info;
            v.Seed = e.Seed;
            v.Stage = ConnectionAck.SecondLogin;
            Info = v;
            Socket?.Disconnect(reuseSocket: true);
            if (Socket == null || Socket.Blocking)
                await Main();
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
            Assistant.State.Send(PPlayServer.Instantiate((byte)shard.Index));
        }
        private static void Network_OnAttach(ConnectionEventArgs connection)
        {
            switch (Assistant.Info.Stage)
            {
                case ConnectionAck.FirstLogin:
                    PSeed.SendBy(State, State.Address.ToUInt32());
                    State.Login(new Account(Assistant.Info.Username, Assistant.Info.Password));
                    break;

                case ConnectionAck.SecondLogin:
                    Logger.Log(typeof(Assistant), "Sending ack response...", ConsoleColor.Magenta);
                    Int32 v = Info.Seed;
                    Span<byte> b = stackalloc byte[4] {
                            (byte)(v >> 0x18),	// 24
										(byte)(v >> 0x10),	// 16 
										(byte)(v >> 0x08),	//  8
										(byte)(v >> 0x00)	// 00
                        };
                    Socket.Send(b);
                    State.Crypto = new GameCrypto();
                    State.Send(SecondLoginAuth.Instantiate(new SecondLoginAuthEventArgs(State, Info.Username, Info.Password, Info.Seed)));
                    break; // @Taz - Add break point here
            }
        }
        private static void Network_OnDetach(NetState e)
        {
            Network.OnDetach -= Network_OnDetach;
            Logger.Log("Network detached state", ConsoleColor.Yellow);

            // Reconnect with the seed
            Task.Run(Assistant.Main);
        }
    }
}
