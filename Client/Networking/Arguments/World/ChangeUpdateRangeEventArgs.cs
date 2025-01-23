namespace Client.Networking.Arguments;
using Client.Game;
public sealed class ChangeUpdateRangeEventArgs : EventArgs
{
    [PacketHandler(0xC8, length: 2, ingame: true)]
    private static event PacketEventHandler<ChangeUpdateRangeEventArgs>? Update;
    public NetState State { get; }
    public byte Value { get; }
    private ChangeUpdateRangeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = ip.ReadByte();
    }
    static ChangeUpdateRangeEventArgs() => Update += ChangeUpdateRangeEventArgs_OnUpdate;
    private static void ChangeUpdateRangeEventArgs_OnUpdate(ChangeUpdateRangeEventArgs e) => World.Instance.TileRange = e.Value;
}