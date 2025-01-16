namespace Client.Networking.Incoming;
public sealed class ContainerContentUpdateEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get; }
    public ushort ID { get; }
    public ushort Amount { get; }
    public short X { get; }
    public short Y { get; }
    public int Parent { get; }
    public short Hue { get; }
    internal ContainerContentUpdateEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Serial = pvSrc.ReadInt32();
        ID = pvSrc.ReadUInt16();
        pvSrc.ReadByte();   //  signed, itemID offset
        Amount = pvSrc.ReadUInt16();
        X = pvSrc.ReadInt16();
        Y = pvSrc.ReadInt16();
        Parent = pvSrc.ReadInt32();
        Hue = pvSrc.ReadInt16();
    }
}