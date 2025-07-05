using Client.Game;

namespace Client.Networking.Arguments;
public sealed class LaunchBrowserEventArgs : EventArgs
{
    [PacketHandler(0xA5, length: -1, ingame: true)]
    public static event PacketEventHandler<LaunchBrowserEventArgs>? Update;
    public NetState State { get; }
    public string? URL { get; }
    internal LaunchBrowserEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        URL = ip.ReadString();
    }
    static LaunchBrowserEventArgs() => Update += LaunchBrowserEventArgs_Update;

    private static void LaunchBrowserEventArgs_Update(LaunchBrowserEventArgs e) => Shard.LaunchBrowser(e.URL);
}