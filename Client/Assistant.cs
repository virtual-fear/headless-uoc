namespace Client
{
    using System.Net;
    using Client.Accounting;
    using Client.Game;
    using Client.Game.Data;
    using Client.Networking;
    using Client.Networking.Arguments;
    using Client.Networking.Data;
    using Client.Networking.Packets;

    /// <summary>
    ///     An event driven network client, built for ModernUO.
    ///     <para>Currently supporting <c>.NET Framework v6.0</c></para>
    ///     It is recommended to use this class to handle and maintain assisted network events.
    ///     <para>Game events that are network related can be subscribed outside of this class to 
    ///     interact parallel with external game objects.</para>
    /// </summary>
    public partial class Assistant : Network
    {
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
            Network.OnAttach += Network_OnAttach;
            Shard.OnUpdate_ServerList += Shard_UpdateServerList;
            Shard.OnServerAck += Shard_OnServerAck;
            Shard.OnCharacterList += Shard_UpdateCharacterList;

            PacketHandlers.RegisterAttributes();
            PacketHandlers.RegisterAttributeEvents();
        }

        private static void Shard_UpdateCharacterList(CharacterListEventArgs e)
        {
            CharInfo[]? characterList = e.Characters?.ToArray();
            if (characterList == null || characterList.Length == 0)
            {
                Logger.LogError($"{nameof(Assistant)}: No characters to select.");
                e.State.Detach();
                return;
            }
            
            CharInfo? firstCharacter = characterList.FirstOrDefault();
            if (firstCharacter == null)
                throw new ArgumentNullException(nameof(firstCharacter));

            firstCharacter.Play();
            Network.State?.Slice();
        }
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
        private static void Shard_UpdateServerList(ServerListEventArgs e)
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
            Logger.Log("  > Connecting to the first shard available!", LogColor.Info);
            if (Network.State == null)
            {
                throw new InvalidOperationException("Invalid network state",
                    innerException: new ArgumentNullException(nameof(Network.State)));
            }
            Network.State.Send(PPlayServer.Instantiate((byte)shard.Index));
        }
        private static void Network_OnAttach(ConnectionEventArgs e)
        {
            Logger.Log(Application.Name, $"{e.State.Address} attached to network state.", LogColor.Info);
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
                if (Network.State == null)
                    throw new ArgumentNullException(nameof(Network.State));

                PInitialSeed.SendBy(Network.State);
                Network.State.Slice();
                Network.State.Login(new Account(username, password));
                e.IsAllowed = true;
            }
        }
    }
}
