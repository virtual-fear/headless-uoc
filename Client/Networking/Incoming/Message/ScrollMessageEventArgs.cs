namespace Client.Networking.Incoming;
public sealed class ScrollMessageEventArgs : EventArgs
{
    public NetState State { get; }
    public ScrollMessageEventArgs(NetState state) => State = state;
    public byte Type { get; set; }
    public int Tip { get; set; }
    public string? Text { get; set; }
}
public partial class Message
{
    public static event PacketEventHandler<ScrollMessageEventArgs>? Chat_OnScrollMessage;

    [PacketHandler(0xA6, length: -1, ingame: true)]
    protected static void ReceivedMessage_Scroll(NetState ns, PacketReader pvSrc)
    {
        ScrollMessageEventArgs e = new(ns);
        e.Type = pvSrc.ReadByte();
        e.Tip = pvSrc.ReadInt32();
        e.Text = pvSrc.ReadString(pvSrc.ReadUInt16());
        Chat_OnScrollMessage?.Invoke(e);
    }
}