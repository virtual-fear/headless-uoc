namespace Client.Networking.Incoming;
public sealed class MapCommandEventArgs : EventArgs
{
    public NetState State { get; }
    public int MapItem { get; set; }
    public byte Command { get; set; }
    public byte Number { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    internal MapCommandEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        MapItem = ip.ReadInt32();
        Command = ip.ReadByte();
        Number = ip.ReadByte();
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
    }
}