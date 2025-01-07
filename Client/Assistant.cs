namespace Client
{
    using System.Net;
    using Client.Networking;
    using Client.Networking.Data;
    using Client.Networking.Incoming;
    using Client.Networking.Outgoing;
    using static Client.Networking.Incoming.PacketSink;

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
        public static readonly Version ClientVersion = new Version(7, 0, 106, 21);

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
        }

        public static void Configure()
        {
            PacketHandlers.Configure();

            // TODO: Create a class to handle the Configuration setup of PacketSink
            // so when the app is run, it can determine what events need to be used
            PacketSink.ServerList += ReceivedServerList_0xA8;
            PacketSink.ServerAck += ReceivedServerAck_0x8C;
        }

        private static async void ReceivedServerAck_0x8C(ServerAckEventArgs e)
        {
            Logger.Log(typeof(Assistant),
                       "Received acknowledgement from the server.", color: LogColor.Info);
            Logger.Log($"{new string(' ', nameof(Assistant).Length+-4)}Seed:0x{e.Seed:X4}", color: LogColor.Success);
            var v = Network.Info;
            v.Seed = e.Seed;
            v.Stage = ConnectionAck.SecondLogin;
            Info = v;
            Socket?.Disconnect(reuseSocket: true);
            await Task.CompletedTask;
        }

        //[Obsolete("This event method is not used in the original code (used only for testing purposes)", error: false)]
        private static void ReceivedServerList_0xA8(ServerListReceivedEventArgs e)
        {
            var shard = e.ServerListEntries.FirstOrDefault();
            if (shard == null)
            {
                Logger.Log("No shards are currently available", LogColor.Warning);
                return;
            }
            Logger.Log("  > Connecting to the first shard available!", LogColor.DarkMagenta);
            if (Network.State == null)
            {
                throw new InvalidOperationException("Invalid network state",
                    innerException: new ArgumentNullException(nameof(Network.State)));
            }
            Network.State.Send(PPlayServer.Instantiate((byte)shard.Index));
        }

    }
}
