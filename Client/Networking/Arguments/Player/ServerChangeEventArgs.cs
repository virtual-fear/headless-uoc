namespace Client.Networking.Arguments;
public sealed class ServerChangeEventArgs : EventArgs
{
    public NetState State { get; }
    public short X { get; set; }
    public short Y { get; set; }
    public short Z { get; set; }
    public short XMap { get; set; }
    public short YMap { get; set; }
    internal ServerChangeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
        Z = ip.ReadInt16();
        ip.Seek(5, SeekOrigin.Current);
        XMap = ip.ReadInt16();
        YMap = ip.ReadInt16();
    }
}