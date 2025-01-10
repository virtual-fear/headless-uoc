namespace Client.Networking.Incoming;
public sealed class CurrentTimeEventArgs : EventArgs
{
    public NetState State { get; }
    public CurrentTimeEventArgs(NetState state) => State = state;
    public TimeSpan Span { get; set; }
}
public partial class Shard
{
    public static event PacketEventHandler<CurrentTimeEventArgs>? UpdateCurrentTime;

    [PacketHandler(0x5B, length: 4, ingame: false)]
    internal static void Received_CurrentTime(NetState ns, PacketReader pvSrc)
    {
        CurrentTimeEventArgs e = new CurrentTimeEventArgs(ns);
        byte h = pvSrc.ReadByte();
        byte m = pvSrc.ReadByte();
        byte s = pvSrc.ReadByte();
        e.Span = new TimeSpan(h, m, s);
        UpdateCurrentTime?.Invoke(e);
    }
}