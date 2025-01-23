namespace Client.Networking.Arguments;
using Client.Game;
public sealed class ClosedGumpEventArgs : EventArgs
{
    [PacketHandler(0x04, length: 13, ingame: true, extCmd: true)]
    private static event PacketEventHandler<ClosedGumpEventArgs>? Update;
    public NetState State { get; }
    public int TypeID { get; }
    public int ButtonID { get; }
    private ClosedGumpEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        TypeID = ip.ReadInt32();
        ButtonID = ip.ReadInt32();
    }
    static ClosedGumpEventArgs() => Update += ClosedGumpEventArgs_Update;
    private static void ClosedGumpEventArgs_Update(ClosedGumpEventArgs e)
        => Gump.Close(e.State, e.TypeID, e.ButtonID);
}