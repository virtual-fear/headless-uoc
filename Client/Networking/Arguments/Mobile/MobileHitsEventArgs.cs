namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MobileHitsEventArgs : EventArgs
{
    [PacketHandler(0xA1, length: 9, ingame: true)]
    private static event PacketEventHandler<MobileHitsEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public short HitsMax { get; }
    public short Hits { get; }
    internal MobileHitsEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        HitsMax = ip.ReadInt16();
        Hits = ip.ReadInt16();
    }
    static MobileHitsEventArgs() => Update += MobileHitsEventArgs_Update;
    private static void MobileHitsEventArgs_Update(MobileHitsEventArgs e)
        => Mobile.Acquire(e.Serial).UpdateHealth(e.Hits, e.HitsMax);
}