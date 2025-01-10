namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public static event PacketEventHandler<NullFastwalkStackEventArgs>? UpdateNullFastwalkStack;
    public sealed class NullFastwalkStackEventArgs : EventArgs
    {
        public NetState State { get; }
        public NullFastwalkStackEventArgs(NetState state) => State = state;
        public byte[]? Buffer { get; set; }
    }
    protected static class NullFastwalkStack
    {
        [PacketHandler(0x1D, length: 5, ingame: true, extCmd: true)]
        internal static void Extended_NullFastwalkStack(NetState ns, PacketReader pvSrc)
        {
            NullFastwalkStackEventArgs e = new(ns);
            e.Buffer = pvSrc.ReadBytes(6 * sizeof(int));
            UpdateNullFastwalkStack?.Invoke(e);
        }

    }
}
