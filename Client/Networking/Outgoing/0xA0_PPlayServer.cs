namespace Client.Networking.Outgoing;
internal sealed class PPlayServer : Packet
{
    private PPlayServer() : base(0xA0, 3) { }
    public static Packet Instantiate(byte index)
    {
        Packet packet = new PPlayServer();
        packet.Stream.Write((short)index);
        packet.Stream.Fill();
        return packet;
    }
}