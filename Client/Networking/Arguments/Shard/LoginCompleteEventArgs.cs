namespace Client.Networking.Arguments;
public sealed class LoginCompleteEventArgs : EventArgs
{
    public NetState State { get; }
    internal LoginCompleteEventArgs(NetState state, PacketReader ip) => State = state;
}