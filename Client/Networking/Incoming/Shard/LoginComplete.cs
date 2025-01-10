namespace Client.Networking.Incoming;
public sealed class LoginCompleteEventArgs : EventArgs
{
    public NetState State { get; }
    public LoginCompleteEventArgs(NetState state) => State = state;
}
public partial class Shard
{
    public static event PacketEventHandler<LoginCompleteEventArgs>? OnLoginComplete;

    [PacketHandler(0x55, length: 1, ingame: true)]
    protected static void Received_LoginComplete(NetState ns, PacketReader pvSrc)
    {
        LoginCompleteEventArgs e = new(ns);
        OnLoginComplete?.Invoke(e);
    }
}
