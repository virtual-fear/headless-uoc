namespace Client.Networking.Incoming;
public sealed class UnicodeMessageEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get; set; }
    public short Graphic { get; set; }
    public byte MessageType { get; set; }
    public short Hue { get; set; }
    public short Font { get; set; }
    public string? Language { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
    internal UnicodeMessageEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        pvSrc.Seek(0, SeekOrigin.Begin);
        Serial = pvSrc.ReadInt32();
        Graphic = pvSrc.ReadInt16();
        MessageType = pvSrc.ReadByte();
        Hue = pvSrc.ReadInt16();
        Font = pvSrc.ReadInt16();
        Language = pvSrc.ReadString(4);
        Name = pvSrc.ReadString(30);
        Text = pvSrc.ReadUnicodeString();
    }

}