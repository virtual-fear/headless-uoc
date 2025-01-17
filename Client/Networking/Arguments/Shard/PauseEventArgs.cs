namespace Client.Networking.Arguments;
public sealed class PauseEventArgs : EventArgs
{
    public NetState State { get; }
    public bool Supported { get; } = false;
    internal PauseEventArgs(NetState state, PacketReader ip) => State = state;
}