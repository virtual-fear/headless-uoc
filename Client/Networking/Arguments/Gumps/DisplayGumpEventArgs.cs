namespace Client.Networking.Arguments;
public sealed class DisplayGumpEventArgs : EventArgs
{
    public NetState? State { get; }
    public int Serial { get; }
    public int TypeID { get; }
    public int GumpX { get; }
    public int GumpY { get; }
    public string? Layout { get; }
    public string[]? Text { get; }
    internal DisplayGumpEventArgs(NetState state, PacketReader ip)
    {
        Serial = ip.ReadInt32();
        TypeID = ip.ReadInt32();
        GumpX = ip.ReadInt32();
        GumpY = ip.ReadInt32();
        Layout = ip.ReadString(ip.ReadUInt16());
        string[] text = new string[ip.ReadUInt16()];
        for (int i = 0; i < text.Length; ++i)
        {
            int l;
            string v;

            l = ip.ReadUInt16();
            v = ip.ReadUnicodeString(l);

            text[i] = v;
        }
        Text = text;
    }
}