namespace Client
{
    using System.Net;
    using Client.Accounting;
    using Client.Networking;
    using Client.Networking.Arguments;
    using Client.Networking.Data;
    using Client.Networking.Packets;

    /// <summary>
    ///     An event driven network client, built for ModernUO.
    ///     <para>Currently supporting <c>.NET Framework v6.0</c></para>
    ///     It is recommended to use this class to handle and maintain assisted network events.
    /// </summary>
    public partial class Assistant : Network
    {
        static Assistant()
        {
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
            Network.OnDetach += Network_OnDetach;

            PacketHandlers.RegisterAttributeEvents();
        }

        /// <summary>
        ///     Reconnect to the server after sending our seed and login packet
        /// </summary>
        private static void Network_OnDetach(NetState ns)
        {
            Logger.Log(Application.Name, $"{ns.Address} detached network state.", LogColor.Info);

            // Reconnect with the seed
            Task.Run(AsyncConnect);
        }

        /// <summary>
        ///     Sends the inital seed and login packet when the network gets attached
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        private static void Network_OnAttach(ConnectionEventArgs e)
        {
            Logger.Log(Application.Name, $"{e.State.Address} attached network state.", LogColor.Info);
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
