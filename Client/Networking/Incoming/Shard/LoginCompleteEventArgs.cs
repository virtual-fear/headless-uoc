namespace Client.Networking.Incoming;
public sealed class LoginCompleteEventArgs : EventArgs
{
    public NetState State { get; }
    internal LoginCompleteEventArgs(NetState state) => State = state;
}