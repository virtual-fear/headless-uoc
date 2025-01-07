namespace Client.Networking.Incoming;
using static PacketSink;
public partial class PacketSink
{
    public sealed class LaunchBrowserEventArgs : EventArgs
    {
        public NetState State { get; }
        public LaunchBrowserEventArgs(NetState state) => State = state;
        public string URL { get; set; }
    }

    public static event PacketEventHandler<LaunchBrowserEventArgs> LaunchBrowser;
    public static void InvokeLaunchBrowser(LaunchBrowserEventArgs e) => LaunchBrowser?.Invoke(e);
}

public static class UpdatedLaunchBrowser
{
    public static void Configure() => Register(0xA5, -1, true, new OnPacketReceive(ReceiveUrl));
    private static void ReceiveUrl(NetState ns, PacketReader pvSrc) => PacketSink.InvokeLaunchBrowser(new LaunchBrowserEventArgs(ns) { URL = pvSrc.ReadString() });
    static void Register(byte packetID, int length, bool ingame, OnPacketReceive onReceive) => PacketHandlers.Register(packetID, length, ingame, onReceive);
}
