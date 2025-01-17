namespace Client.Networking.Arguments;
public sealed class ClosedGumpEventArgs : EventArgs
{
    public NetState State { get; }
    public int TypeID { get; }
    public int ButtonID { get; }
    internal ClosedGumpEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        TypeID = ip.ReadInt32();
        ButtonID = ip.ReadInt32();
    }
}