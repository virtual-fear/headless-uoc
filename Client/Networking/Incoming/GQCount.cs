namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public static event PacketEventHandler<GQCountEventArgs>? UpdateGQCount;
    public sealed class GQCountEventArgs : EventArgs
    {
        public NetState State { get; }
        public GQCountEventArgs(NetState state) => State = state;
        public short Unk { get; set; }
        public int Count { get; set; }
    }
    protected static class GQCount
    {
        [PacketHandler(0xCB, length: 7, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            GQCountEventArgs e = new(ns);
            e.Unk = pvSrc.ReadInt16();
            e.Count = pvSrc.ReadInt32();
            UpdateGQCount?.Invoke(e);
        }
    }
}