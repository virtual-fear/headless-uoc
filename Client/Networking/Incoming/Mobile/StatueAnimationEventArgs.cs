namespace Client.Networking.Incoming;
public sealed class StatueAnimationEventArgs : EventArgs
{
    public NetState State { get; }
    public int Mobile { get; }
    public byte Status { get; }
    public byte Animation { get; }
    public byte Frame { get; }
    internal StatueAnimationEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.Seek(3, SeekOrigin.Current);
        //pvSrc.ReadInt16();  //  0x19
        //pvSrc.ReadByte();   //  0x05
        Mobile = ip.ReadInt32();
        ip.Seek(2, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        //pvSrc.ReadByte();   //  0xFF
        Status = ip.ReadByte();
        ip.Seek(1, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        Animation = ip.ReadByte();
        ip.Seek(1, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        Frame = ip.ReadByte();
    }

}