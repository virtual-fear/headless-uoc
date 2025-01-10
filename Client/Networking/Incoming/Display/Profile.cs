namespace Client.Networking.Incoming.Display;
using Client.Game.Context;
using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<DisplayProfileEventArgs>? DisplayProfile;
    public sealed class DisplayProfileEventArgs : EventArgs
    {
        public NetState State { get; }
        public DisplayProfileEventArgs(NetState state) => State = state;
        public MobileContext Mobile { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string Body { get; set; }
    }
    protected static class Profile
    {
        [PacketHandler(0xB8, length: -1, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            DisplayProfileEventArgs e = new DisplayProfileEventArgs(ns);
            e.Mobile = MobileContext.Acquire((Serial)pvSrc.ReadUInt32());
            e.Header = pvSrc.ReadString();
            e.Footer = pvSrc.ReadUnicodeString();
            e.Body = pvSrc.ReadUnicodeString();
            DisplayProfile?.Invoke(e);
        }
    }
}