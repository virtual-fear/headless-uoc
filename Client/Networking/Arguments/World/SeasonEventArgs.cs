namespace Client.Networking.Arguments;
public sealed class SeasonChangeEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Value { get; }
    public bool Sound { get; }
    internal SeasonChangeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = ip.ReadByte();
        Sound = ip.ReadBoolean();
    }
}