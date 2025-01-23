namespace Client.Networking.Arguments;
using Client.Game;
public sealed class DisplayPackedGumpEventArgs : EventArgs
{
    [PacketHandler(0xDD, length: -1, ingame: true)]
    private static event PacketEventHandler<DisplayPackedGumpEventArgs>? Update;
    public NetState State { get; }
    public int Serial { get; }
    public int TypeID { get; }
    public int GumpX { get; }
    public int GumpY { get; }
    public string? Layout { get; }
    public string[]? Text { get; }
    private DisplayPackedGumpEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
        TypeID = ip.ReadInt32();
        GumpX = ip.ReadInt32();
        GumpY = ip.ReadInt32();
        ip = Gump.GetCompressedReader(ip);
        Layout = ip.ReadString();
        string[] text = new string[ip.ReadInt32()];
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

    static DisplayPackedGumpEventArgs() => Update += DisplayPackedGumpEventArgs_Update;
    private static void DisplayPackedGumpEventArgs_Update(DisplayPackedGumpEventArgs e)
        => Gump.Display(e.State, e.Serial, e.TypeID, e.GumpX, e.GumpY, e.Layout, e.Text);
}