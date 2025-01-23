namespace Client.Networking.Arguments;
public sealed class LoginDelayEventArgs : EventArgs
{
    [PacketHandler(0xFD, length: -1, ingame: false)]
    public static event PacketEventHandler<LoginDelayEventArgs>? Update;
    public NetState State { get; }
    internal LoginDelayEventArgs(NetState state) => State = state;

    static LoginDelayEventArgs() => Update += LoginDelayEventArgs_Update;
    private static void LoginDelayEventArgs_Update(LoginDelayEventArgs e)
        => throw new NotImplementedException();
}