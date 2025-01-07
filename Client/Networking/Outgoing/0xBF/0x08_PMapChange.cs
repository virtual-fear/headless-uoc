namespace Client.Networking.Outgoing;
public sealed class PMapChange : Packet
{
    private PMapChange() : base(0xBF) => base.Stream.Write((short)0x08);
    private static Packet Instantiate(NetState state)
    {
        Packet packet = new PMapChange();
        //packet.Stream.Write((byte)state.Mobile.MapIndex);
        return packet;
    }

    [Obsolete("MapIndex not introduced into Mobile", true)]
    public static void SendBy(NetState state) => state.Send(PMapChange.Instantiate(state));
}
