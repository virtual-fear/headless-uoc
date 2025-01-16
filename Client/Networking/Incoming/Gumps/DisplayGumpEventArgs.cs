namespace Client.Networking.Incoming;
public sealed class DisplayGumpEventArgs : EventArgs
{
    public NetState? State { get; }
    public int Serial { get; }
    public int TypeID { get; }
    public int GumpX { get; }
    public int GumpY { get; }
    public string? Layout { get; }
    public string[]? Text { get; }
    public bool Packed { get; }
    internal DisplayGumpEventArgs(NetState state, PacketReader ip, bool packed = false)
    {
        Serial = ip.ReadInt32();
        TypeID = ip.ReadInt32();
        GumpX = ip.ReadInt32();
        GumpY = ip.ReadInt32();
        if (Packed = packed)
            ip = Gumps.GetCompressedReader(ip);
        Layout = packed ? ip.ReadString() : ip.ReadString(ip.ReadUInt16());
        string[] text = new string[packed ? ip.ReadInt32() : ip.ReadUInt16()];
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