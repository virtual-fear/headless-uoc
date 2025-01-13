namespace Client
{
    using System.Net;
    using Client.Networking;
    using Client.Networking.Data;
    using Client.Networking.Incoming;
    using Client.Networking.Outgoing;

    /// <summary>
    ///     An event driven network client, built for ModernUO.
    ///     <para>Currently supporting <c>.NET Framework v6.0</c></para>
    ///     It is recommended to use this class to handle and maintain the network events.
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
            PacketHandlers.RegisterAttributes();

            Shard.UpdateServerList += Shard_UpdateServerList;
            Shard.OnServerAck += Shard_OnServerAck;
        }

        /// <summary>
        ///     ReceivedServerAck : 0x8C
        /// </summary>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static async void Shard_OnServerAck(ServerAckEventArgs e)
        {
            Logger.Log(typeof(Assistant),
                       "Received acknowledgement from the server.", color: LogColor.Info);
            Logger.Log($"{new string(' ', nameof(Assistant).Length + -4)}Seed:0x{e.Seed:X4}", color: LogColor.Success);
            var v = Network.Info;
            v.Seed = e.Seed;
            v.Stage = ConnectionAck.SecondLogin;
            Info = v;
            Socket?.Disconnect(reuseSocket: true);
            await Task.CompletedTask;
        }

        //[Obsolete("This event method is not used in the original code (used only for testing purposes)", error: false)]
        private static void Shard_UpdateServerList(ServerListReceivedEventArgs e)
        {
            if (e.ServerListEntries.Length == 0)
            {
                Logger.Log("No shard entries are currently available", LogColor.Warning);
                return;
            }
            var shard = e.ServerListEntries.FirstOrDefault();
            if (shard.Name.Length == 0)
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
