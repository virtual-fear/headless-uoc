namespace Client.Networking.Incoming;
public sealed class GQCountEventArgs : EventArgs
{
    public NetState State { get; }
    public GQCountEventArgs(NetState state) => State = state;
    public short Unk { get; set; }
    public int Count { get; set; }
}
public partial class Shard
{
    public static event PacketEventHandler<GQCountEventArgs>? UpdateGQCount;

    [PacketHandler(0xCB, length: 7, ingame: true)]
    protected static void Received_ProtocolExtension_0xF0(NetState ns, PacketReader pvSrc)
    {
        GQCountEventArgs e = new(ns);
        e.Unk = pvSrc.ReadInt16();
        e.Count = pvSrc.ReadInt32();
        UpdateGQCount?.Invoke(e);
    }
}
