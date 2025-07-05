using Client.Game;

namespace Client.Networking.Arguments;
public sealed class LoginCompleteEventArgs : EventArgs
{
    [PacketHandler(0x55, length: 1, ingame: true)]
    public static event PacketEventHandler<LoginCompleteEventArgs>? Update;
    public NetState State { get; }
    public Mobile? Mobile => State.Mobile;
    internal LoginCompleteEventArgs(NetState state, PacketReader ip) => State = state;
}