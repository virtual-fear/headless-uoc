namespace Client.Networking.Incoming;
public sealed class DragEffectEventArgs : EventArgs
{
    public NetState State { get; }
    public short ItemID { get; }
    public short Hue { get; }
    public short Amount { get; }
    public int SourceSerial { get; }
    public int TargetSerial { get; }
    public short SourceX { get; }
    public short SourceY { get; }
    public sbyte SourceZ { get; }
    public short TargetX { get; }
    public short TargetY { get; }
    public sbyte TargetZ { get; }
    internal DragEffectEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ItemID = ip.ReadInt16();
        ip.ReadByte();
        Hue = ip.ReadInt16();
        Amount = ip.ReadInt16();
        SourceSerial = ip.ReadInt32();
        SourceX = ip.ReadInt16();
        SourceY = ip.ReadInt16();
        SourceZ = ip.ReadSByte();
        TargetSerial = ip.ReadInt32();
        TargetX = ip.ReadInt16();
        TargetY = ip.ReadInt16();
        TargetZ = ip.ReadSByte();
    }
}