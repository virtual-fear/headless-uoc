namespace Client.Networking.Arguments;
public sealed class ContainerDisplayEventArgs : EventArgs
{
    public NetState State { get; }
    public int Container { get; }
    public short GumpID { get; }
    internal ContainerDisplayEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Container = ip.ReadInt32();
        GumpID = ip.ReadInt16();
    }
}