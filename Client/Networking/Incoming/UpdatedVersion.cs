namespace Client.Networking.Incoming
{
    public static class UpdatedVersion
    {
        #region Outgoing Packets

        public sealed class AssistantVersion : Packet
        {
            public static void SendBy(NetState state, PacketReader pvSrc)
            {
                state.Send(AssistantVersion.Instantiate(state, pvSrc));
            }

            private static Packet Instantiate(NetState state, PacketReader pvSrc)
            {
                Packet packet = new AssistantVersion();
                packet.Stream.Write((int)pvSrc.ReadInt32());
                packet.Stream.Write(state.Version.ToString());
                packet.Stream.Fill(sizeof(byte));
                return packet;
            }

            /// <summary>
            /// Requesting Assistant
            /// </summary>
            private AssistantVersion()
                : base(0xBE)
            {
            }
        }

        public sealed class ClientVersion : Packet
        {
            public static void SendBy(NetState state)
            {
                state.Send(ClientVersion.Instantiate(state));
            }

            private static Packet Instantiate(NetState state)
            {
                Packet packet = new ClientVersion();
                packet.Stream.Write(state.Version.ToString());
                packet.Stream.Fill();
                return packet;
            }

            private ClientVersion()
                : base(0xBD)
            {
            }
        }

        #endregion

        public static void Configure()
        {
            Register(0xBE, 07, true, new OnPacketReceive(AssistVer));
        }

        // TODO: Register packet handler
        private static void ClientVersionReq(NetState ns, PacketReader pvSrc)
        {
            ClientVersion.SendBy(ns);
        }

        private static void AssistVer(NetState ns, PacketReader pvSrc) => AssistantVersion.SendBy(ns, pvSrc);
        public static void Register(int packetID, int length, bool ingame, OnPacketReceive receive) => PacketHandlers.Register(packetID, length, ingame, receive);
    }
}
