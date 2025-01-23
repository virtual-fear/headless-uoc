namespace Client.Networking.Arguments;
using Client.Game;
public sealed class DisplayGumpEventArgs : EventArgs
{
    [PacketHandler(0xB0, length: -1, ingame: true)]
    private static event PacketEventHandler<DisplayGumpEventArgs>? Update;
    public NetState State { get; }
    public int Serial { get; }
    public int TypeID { get; }
    public int GumpX { get; }
    public int GumpY { get; }
    public string? Layout { get; }
    public string[]? Text { get; }
    private DisplayGumpEventArgs(NetState state, PacketReader ip)
    {
        State = state;
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
    static DisplayGumpEventArgs() => Update += DisplayGumpEventArgs_Update;
    private static void DisplayGumpEventArgs_Update(DisplayGumpEventArgs e)
        => Gump.Display(e.State, e.Serial, e.TypeID, e.GumpX, e.GumpY, e.Layout, e.Text);
}