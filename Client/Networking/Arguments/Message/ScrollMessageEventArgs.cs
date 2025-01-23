namespace Client.Networking.Arguments;
using Client.Game;
public sealed class ScrollMessageEventArgs : EventArgs
{
    [PacketHandler(0xA6, length: -1, ingame: true)]
    private static event PacketEventHandler<ScrollMessageEventArgs>? Update;
    public NetState State { get; }
    public byte Type { get; }
    public int Tip { get; }
    public string? Text { get; }
    private ScrollMessageEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Type = ip.ReadByte();
        Tip = ip.ReadInt32();
        Text = ip.ReadString(ip.ReadUInt16());
    }

    static ScrollMessageEventArgs() => Update += ScrollMessageEventArgs_Update;
    private static void ScrollMessageEventArgs_Update(ScrollMessageEventArgs e)
        => Message.AddScrollEntry(e.Type, e.Tip, e.Text);
}