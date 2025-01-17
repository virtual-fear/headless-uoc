namespace Client.Networking.Packets;
public sealed class PQueryGuardline : Packet
{
    private PQueryGuardline() : base(240) { }
    public static void SendBy(NetState state) => state.Send(PQueryGuardline.Instantiate());
    private static Packet Instantiate()
    {
        Packet packet = new PQueryGuardline();
        packet.Stream.Write(0x4);
        return packet;
    }
}
