namespace Client.Networking.Outgoing;
public sealed class PGumpButton : Packet
{
    private PGumpButton() : base(0xB1) { }
    public static Packet Instantiate(int serial, int dialogID, int buttonID)
    {
        Packet packet = new PGumpButton();
        packet.Stream.Write((int)serial);
        packet.Stream.Write((int)dialogID);
        packet.Stream.Write((int)buttonID);
        packet.Stream.Fill(2);
        return packet;
    }
}