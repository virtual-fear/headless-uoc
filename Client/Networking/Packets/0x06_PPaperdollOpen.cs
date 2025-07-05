namespace Client.Networking.Packets;
public sealed class PPaperdollOpen : Packet
{
    private PPaperdollOpen() : base(0x06, 5) { }
    public static Packet Instantiate(int serial)
    {
        Packet packet = new PPaperdollOpen();
        packet.Stream.Write(serial | 0x7FFFFFFF);
        return packet;
    }
}
