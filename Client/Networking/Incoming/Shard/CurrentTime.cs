namespace Client.Networking.Incoming.Shard;
public partial class PacketHandlers
{
    public static event PacketEventHandler<CurrentTimeEventArgs>? Shard_CurrentTime;
    public sealed class CurrentTimeEventArgs : EventArgs
    {
        public NetState State { get; }
        public CurrentTimeEventArgs(NetState state) => State = state;
        public TimeSpan Span { get; set; }
    }
    protected static class CurrentTime
    {
        [PacketHandler(0x5B, length: 4, ingame: false)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            CurrentTimeEventArgs e = new CurrentTimeEventArgs(ns);
            byte h = pvSrc.ReadByte();
            byte m = pvSrc.ReadByte();
            byte s = pvSrc.ReadByte();
            e.Span = new TimeSpan(h, m, s);
            Shard_CurrentTime?.Invoke(e);
        }

    }
}