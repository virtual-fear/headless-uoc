namespace Client.Networking.Incoming;
public sealed class RejectedLoginEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Command { get; }
    internal RejectedLoginEventArgs(NetState state, byte cmd)
    {
        State = state;
        Command = cmd;
    }
}