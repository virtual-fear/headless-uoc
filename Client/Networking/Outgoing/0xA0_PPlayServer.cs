namespace Client.Networking.Outgoing;
public sealed class PPlayServer : Packet
{
    private PPlayServer() : base(0xA0, 3) => Encode = false;
    public static Packet Instantiate(byte index)
    {
        Packet packet = new PPlayServer();
        packet.Stream.Write((short)index);
        packet.Stream.FilltoCapacity();
        return packet;
    }
}