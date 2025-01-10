namespace Client.Networking.Incoming;
public sealed class AsciiMessageEventArgs : EventArgs
    {
        public NetState State { get; }
        public AsciiMessageEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short Graphic { get; set; }
        public byte MessageType { get; set; }
        public short Hue { get; set; }
        public short Font { get; set; }
        public string? Name { get; set; }
        public string? Text { get; set; }
    }
public partial class Message
{
    public static event PacketEventHandler<AsciiMessageEventArgs>? OnASCII;

    [PacketHandler(0x1C, length: -1, ingame: true)]
    protected static void ReceivedMessage_ASCII(NetState ns, PacketReader pvSrc)
    {
        AsciiMessageEventArgs e = new(ns);
        e.Serial = pvSrc.ReadInt32();
        e.Graphic = pvSrc.ReadInt16();
        e.MessageType = pvSrc.ReadByte();
        e.Hue = pvSrc.ReadInt16();
        e.Font = pvSrc.ReadInt16();
        e.Name = pvSrc.ReadString(30); // AsciiFixed
        e.Text = pvSrc.ReadString(); // AsciiNull
        OnASCII?.Invoke(e);
    }
}