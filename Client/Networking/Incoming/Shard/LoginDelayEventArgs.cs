namespace Client.Networking.Incoming;
public sealed class LoginDelayEventArgs : EventArgs
{
    public NetState State { get; }
    internal LoginDelayEventArgs(NetState state) => State = state;
}