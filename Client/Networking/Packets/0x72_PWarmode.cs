namespace Client.Networking.Packets;
public sealed class PWarmode : Packet
{
    private PWarmode() : base(0x72, 5) { }
    public static Packet Instantiate(bool warMode) => Instantiate(warMode, 32, 0);
    public static Packet Instantiate(bool warMode, short unk1, short unk2)
    {
        Packet packet = new PWarmode();
        packet.Stream.Write((bool)warMode);
        packet.Stream.Write((short)unk1);
        packet.Stream.Write((short)unk2);
        return packet;
    }
}