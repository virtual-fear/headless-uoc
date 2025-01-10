namespace Client.Networking.Incoming;
public sealed class NullFastwalkStackEventArgs : EventArgs
{
    public NetState State { get; }
    public NullFastwalkStackEventArgs(NetState state) => State = state;
    public byte[]? Buffer { get; set; }
}
public partial class Shard
{
    public static event PacketEventHandler<NullFastwalkStackEventArgs>? UpdateNullFastwalkStack;

    [PacketHandler(0x1D, length: 5, ingame: true, extCmd: true)]
    internal static void ReceivedExt_NullFastwalkStack(NetState ns, PacketReader pvSrc)
    {
        NullFastwalkStackEventArgs e = new(ns);
        e.Buffer = pvSrc.ReadBytes(6 * sizeof(int));
        UpdateNullFastwalkStack?.Invoke(e);
    }
}
