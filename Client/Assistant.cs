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

            PacketHandlers.RegisterAttributeEvents();
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
