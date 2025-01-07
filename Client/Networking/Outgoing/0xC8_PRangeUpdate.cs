namespace Client.Networking.Outgoing;
public sealed class PRangeUpdate : Packet
{
    private PRangeUpdate() : base(0xC8, 2) => base.Stream.Write(18);
    public static void SendBy(NetState state) => state.Send(new PRangeUpdate());
}