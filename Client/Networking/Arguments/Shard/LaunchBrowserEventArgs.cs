namespace Client.Networking.Arguments;
public sealed class LaunchBrowserEventArgs : EventArgs
{
    public NetState State { get; }
    public string? URL { get; }
    internal LaunchBrowserEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        URL = ip.ReadString();
    }
}