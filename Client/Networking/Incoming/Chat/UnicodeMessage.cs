namespace Client.Networking.Incoming.Chat;
public partial class PacketHandlers
{
    public static event PacketEventHandler<UnicodeMessageEventArgs>? Chat_OnUnicodeMessage;
    public sealed class UnicodeMessageEventArgs : EventArgs
    {
        public NetState State { get; }
        public UnicodeMessageEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short Graphic { get; set; }
        public byte MessageType { get; set; }
        public short Hue { get; set; }
        public short Font { get; set; }
        public string? Language { get; set; }
        public string? Name { get; set; }
        public string? Text { get; set; }
    }
    protected static class UnicodeMessage
    {
        [PacketHandler(0xAE, length: -1, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            UnicodeMessageEventArgs e = new UnicodeMessageEventArgs(ns);
            pvSrc.Trace(true);
            pvSrc.Seek(0, SeekOrigin.Begin);
            e.Serial = pvSrc.ReadInt32();
            e.Graphic = pvSrc.ReadInt16();
            e.MessageType = pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.Font = pvSrc.ReadInt16();
            e.Language = pvSrc.ReadString(4);
            e.Name = pvSrc.ReadString(30);
            e.Text = pvSrc.ReadUnicodeString();
            Chat_OnUnicodeMessage?.Invoke(e);
        }
    }
}