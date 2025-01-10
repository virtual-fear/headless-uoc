namespace Client.Networking.Incoming;
public sealed class LaunchBrowserEventArgs : EventArgs
{
    public NetState State { get; }
    public LaunchBrowserEventArgs(NetState state) => State = state;
    public string? URL { get; set; }
}
public partial class Shard
{
    public static event PacketEventHandler<LaunchBrowserEventArgs>? OnLaunchBrowser;

    [PacketHandler(0xA5, length: -1, ingame: true)]
    public static void Received_LaunchBrowser(NetState ns, PacketReader pvSrc)
    {
        LaunchBrowserEventArgs e = new(ns)
        {
            URL = pvSrc.ReadString()
        };
        OnLaunchBrowser?.Invoke(e);
    }
}
