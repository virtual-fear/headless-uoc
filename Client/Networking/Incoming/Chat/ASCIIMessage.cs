namespace Client.Networking.Incoming.Messages;
public partial class PacketHandlers
{
    public static event PacketEventHandler<AsciiMessageEventArgs>? Chat_OnAsciiMessage;
    public sealed class AsciiMessageEventArgs : EventArgs
    {
        public NetState State { get; }
        public AsciiMessageEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short Graphic { get; set; }
        public byte MessageType { get; set; }
        public short Hue { get; set; }
        public short Font { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
    protected static class ASCIIMessage
    {
        [PacketHandler(0x1C, length: -1, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            AsciiMessageEventArgs e = new AsciiMessageEventArgs(ns);

            e.Serial = pvSrc.ReadInt32();
            e.Graphic = pvSrc.ReadInt16();
            e.MessageType = pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.Font = pvSrc.ReadInt16();
            e.Name = pvSrc.ReadString(30); // AsciiFixed
            e.Text = pvSrc.ReadString(); // AsciiNull
            Chat_OnAsciiMessage?.Invoke(e);
        }
    }
}