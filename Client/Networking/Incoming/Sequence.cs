namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public static event PacketEventHandler<SequenceEventArgs>? UpdateSequence;
    public sealed class SequenceEventArgs : EventArgs
    {
        public NetState State { get; }
        public SequenceEventArgs(NetState state) => State = state;
        public int Value { get; set; }
    }
    protected static class Sequence
    {
        [PacketHandler(0x7B, length: 2, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            SequenceEventArgs e = new(ns);
            e.Value = pvSrc.ReadByte();
            UpdateSequence?.Invoke(e);
        }
    }
}