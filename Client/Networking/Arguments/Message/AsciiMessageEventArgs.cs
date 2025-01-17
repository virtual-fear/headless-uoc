namespace Client.Networking.Arguments;
public sealed class AsciiMessageEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get; }
    public short Graphic { get; }
    public byte MessageType { get; }
    public short Hue { get; }
    public short Font { get; }
    public string? Name { get; }
    public string? Text { get; }
    internal AsciiMessageEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
        Graphic = ip.ReadInt16();
        MessageType = ip.ReadByte();
        Hue = ip.ReadInt16();
        Font = ip.ReadInt16();
        Name = ip.ReadString(30); // AsciiFixed
        Text = ip.ReadString(); // AsciiNull
    }
}