namespace Client.Networking.Arguments;
public sealed class SupportedFeaturesEventArgs : EventArgs
{
    public NetState State { get; }
    public uint Features { get; }
    internal SupportedFeaturesEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Features = ip.ReadUInt32();
    }
}