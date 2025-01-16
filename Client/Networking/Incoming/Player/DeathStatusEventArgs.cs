namespace Client.Networking.Incoming;
public sealed class DeathStatusEventArgs : EventArgs
{
    public NetState State { get; }
    public bool Dead { get; }
    internal DeathStatusEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Dead = ip.ReadByte() == 2;
    }
}