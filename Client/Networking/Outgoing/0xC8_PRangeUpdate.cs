namespace Client.Networking.Outgoing
{
    public sealed class PRangeUpdate : Packet
    {
        private PRangeUpdate()
            : base(0xC8, 0x02) { }

        public static void SendBy(NetState state) => state.Send(PRangeUpdate.Instantiate());
        private static Packet Instantiate()
        {
            Packet packet = new PRangeUpdate();
            packet.Stream.Write(18);
            return packet;
        }
    }
}
