using Client.Game;

namespace Client.Networking.Arguments;
public sealed class GQCountEventArgs : EventArgs
{
    [PacketHandler(0xCB, length: 7, ingame: true)]
    private static event PacketEventHandler<GQCountEventArgs>? Update;
    public NetState State { get; }
    public short Unk { get; }
    public int Count { get; }
    internal GQCountEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Unk = ip.ReadInt16();
        Count = ip.ReadInt32();
    }

    static GQCountEventArgs() => Update += GQCountEventArgs_Update;
    private static void GQCountEventArgs_Update(GQCountEventArgs e) => Shard.GQCount = e.Count;
}