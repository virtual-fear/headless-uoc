namespace Client.Networking.Packets;
public class PPing : Packet
{
    private PPing() : base(0x73, 0x02) { }

    private static PPing[] m_Cache = new PPing[256];
    internal static Packet Write(byte ping)
    {
        if (m_Cache[ping] == null)
            m_Cache[ping] = new PPing();

        Packet packet = m_Cache[ping];
        packet.Stream.Write(ping);
        return packet;
    }
}