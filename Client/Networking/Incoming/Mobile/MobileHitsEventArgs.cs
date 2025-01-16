using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class MobileHitsEventArgs : EventArgs
{
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
}