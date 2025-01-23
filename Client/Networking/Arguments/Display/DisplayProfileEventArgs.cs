namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class DisplayProfileEventArgs : EventArgs
{
    [PacketHandler(0xB8, length: -1, ingame: true)]
    public static event PacketEventHandler<DisplayProfileEventArgs> Update;
    public NetState State { get; }
    public Mobile Mobile { get; }
    public string Header { get; }
    public string Footer { get; }
    public string Body { get; }
    private DisplayProfileEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Mobile = Mobile.Acquire((Serial)ip.ReadUInt32());
        Header = ip.ReadString();
        Footer = ip.ReadUnicodeString();
        Body = ip.ReadUnicodeString();
    }
    static DisplayProfileEventArgs() => Update += DisplayProfileEventArgs_Update;
    private static void DisplayProfileEventArgs_Update(DisplayProfileEventArgs e)
        => Display.ShowProfile(e.State, e.Mobile, e.Header, e.Footer, e.Body);
}