namespace Client.Networking.Arguments;
public sealed class CurrentTimeEventArgs : EventArgs
{
    public NetState State { get; }
    public TimeSpan Span { get; }
    internal CurrentTimeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        byte h = ip.ReadByte();
        byte m = ip.ReadByte();
        byte s = ip.ReadByte();
        Span = new TimeSpan(h, m, s);
    }
}