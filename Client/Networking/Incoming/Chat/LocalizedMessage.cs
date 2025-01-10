namespace Client.Networking.Incoming.Chat;
    public partial class PacketHandlers
{
    public static event PacketEventHandler<LocalizedMessageEventArgs>? Chat_OnLocalizedMessage;
    public sealed class LocalizedMessageEventArgs : EventArgs
    {
        public NetState State { get; }
        public LocalizedMessageEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short Graphic { get; set; }
        public byte MessageType { get; set; }
        public short Hue { get; set; }
        public short Font { get; set; }
        public string? Name { get; set; }
        public string? Text { get; set; }
    }
    protected static class LocalizedMessage
    {
        [PacketHandler(0xC1, length: -1, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            LocalizedMessageEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            e.Graphic = pvSrc.ReadInt16();
            e.MessageType = pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.Font = pvSrc.ReadInt16();
            pvSrc.ReadInt32();  //  e.Number
            e.Name = pvSrc.ReadString(30);
            e.Text = pvSrc.ReadUnicodeStringLE();
            Chat_OnLocalizedMessage?.Invoke(e);
        }

    }
}