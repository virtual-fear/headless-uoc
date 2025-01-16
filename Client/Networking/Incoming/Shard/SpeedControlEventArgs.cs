namespace Client.Networking.Incoming;
public sealed class SpeedControlEventArgs : EventArgs
{
    public NetState State { get; }
    public int Value { get; }
    internal SpeedControlEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = ip.ReadByte();
    }
}