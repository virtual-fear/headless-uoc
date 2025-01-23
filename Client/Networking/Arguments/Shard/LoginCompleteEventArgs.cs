using Client.Game;

namespace Client.Networking.Arguments;
public sealed class LoginCompleteEventArgs : EventArgs
{
    [PacketHandler(0x55, length: 1, ingame: true)]
    public static event PacketEventHandler<LoginCompleteEventArgs>? Update;
    public NetState State { get; }
    internal LoginCompleteEventArgs(NetState state, PacketReader ip) => State = state;
    static LoginCompleteEventArgs() => Update += LoginCompleteEventArgs_Update;
    private static void LoginCompleteEventArgs_Update(LoginCompleteEventArgs e)
    {
        World.LoginComplete(e.State);
    }
}