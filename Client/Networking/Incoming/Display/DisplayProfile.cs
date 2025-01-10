namespace Client.Networking.Incoming;
using Client.Game.Context;
using Client.Game.Data;
public sealed class DisplayProfileEventArgs : EventArgs
{
    public NetState State { get; }
    public DisplayProfileEventArgs(NetState state) => State = state;
    public MobileContext? Mobile { get; set; }
    public string? Header { get; set; }
    public string? Footer { get; set; }
    public string? Body { get; set; }
}
public partial class Display
{
    public static event PacketEventHandler<DisplayProfileEventArgs>? UpdateProfile;

    [PacketHandler(0xB8, length: -1, ingame: true)]
    protected static void ReceivedDisplay_Profile(NetState ns, PacketReader pvSrc)
    {
        DisplayProfileEventArgs e = new DisplayProfileEventArgs(ns);
        e.Mobile = MobileContext.Acquire((Serial)pvSrc.ReadUInt32());
        e.Header = pvSrc.ReadString();
        e.Footer = pvSrc.ReadUnicodeString();
        e.Body = pvSrc.ReadUnicodeString();
        UpdateProfile?.Invoke(e);
    }
}