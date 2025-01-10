namespace Client.Networking.Incoming.Chat;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ScrollMessageEventArgs>? Chat_OnScrollMessage;
    public sealed class ScrollMessageEventArgs : EventArgs
    {
        public NetState State { get; }
        public ScrollMessageEventArgs(NetState state) => State = state;
        public byte Type { get; set; }
        public int Tip { get; set; }
        public string? Text { get; set; }
    }
    protected static class ScrollMessage
    {
        [PacketHandler(0xA6, length: -1, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            ScrollMessageEventArgs e = new(ns);
            e.Type = pvSrc.ReadByte();
            e.Tip = pvSrc.ReadInt32();
            e.Text = pvSrc.ReadString(pvSrc.ReadUInt16());
            Chat_OnScrollMessage?.Invoke(e);
        }
    }
}