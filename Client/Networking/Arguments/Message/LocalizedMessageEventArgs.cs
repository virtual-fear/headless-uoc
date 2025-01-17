namespace Client.Networking.Arguments;
public sealed class LocalizedMessageEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get; }
    public short Graphic { get; }
    public byte MessageType { get; }
    public short Hue { get; }
    public short Font { get; }
    public string? Name { get; }
    public string? Text { get; }
    internal LocalizedMessageEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Serial = pvSrc.ReadInt32();
        Graphic = pvSrc.ReadInt16();
        MessageType = pvSrc.ReadByte();
        Hue = pvSrc.ReadInt16();
        Font = pvSrc.ReadInt16();
        pvSrc.ReadInt32();  //  e.Number
        Name = pvSrc.ReadString(30);
        Text = pvSrc.ReadUnicodeStringLE();
    }

}