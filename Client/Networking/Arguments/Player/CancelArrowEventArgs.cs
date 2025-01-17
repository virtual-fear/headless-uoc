namespace Client.Networking.Arguments;
public sealed class CancelArrowEventArgs : EventArgs
{
    public NetState State { get; }
    internal CancelArrowEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.Seek(5, SeekOrigin.Begin);
    }
}