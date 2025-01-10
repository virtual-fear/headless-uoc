namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ChangeUpdateRangeEventArgs>? OnChangeUpdateRange;
    public sealed class ChangeUpdateRangeEventArgs : EventArgs
    {
        public NetState State { get; }
        public ChangeUpdateRangeEventArgs(NetState state) => State = state;
        public byte Range { get; set; }
    }
    protected static class ChangeUpdateRange
    {
        [PacketHandler(0xC8, length: 2, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            ChangeUpdateRangeEventArgs e = new(ns);
            e.Range = pvSrc.ReadByte();
            OnChangeUpdateRange?.Invoke(e);
        }
    }
}