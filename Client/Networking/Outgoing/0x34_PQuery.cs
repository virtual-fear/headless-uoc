namespace Client.Networking.Outgoing
{
    public class PQuery : Packet
    {
        private PQuery()
            : base(0x34, 0xA) { }

        public static Packet GetStats(int serial) => Instantiate(4, serial);
        private static Packet GetSkills(int serial) => Instantiate(5, serial);
        private static Packet Instantiate(byte type, int serial)
        {
            Packet packet = new PQuery();
            packet.Stream.Fill(4); // sizeof(int)
            packet.Stream.Write(type);
            packet.Stream.Write(serial);
            return packet;
        }

    }

}
