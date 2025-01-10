namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public static event PacketEventHandler<LaunchBrowserEventArgs>? OnLaunchBrowser;
    public sealed class LaunchBrowserEventArgs : EventArgs
    {
        public NetState State { get; }
        public LaunchBrowserEventArgs(NetState state) => State = state;
        public string? URL { get; set; }
    }

    protected static class LaunchBrowser
    {
        [PacketHandler(0xA5, length: -1, ingame: true)]
        public static void ReceiveUrl(NetState ns, PacketReader pvSrc)
        {
            LaunchBrowserEventArgs e = new(ns)
            {
                URL = pvSrc.ReadString()
            };
            OnLaunchBrowser?.Invoke(e);
        }
    }
}