namespace Client.Networking.Incoming;
public sealed class StatueAnimationEventArgs : EventArgs
{
    public NetState State { get; }
    public StatueAnimationEventArgs(NetState state) => State = state;
    public int Mobile { get; set; }
    public byte Status { get; set; }
    public byte Animation { get; set; }
    public byte Frame { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<StatueAnimationEventArgs>? OnStatueAnimation;

    [PacketHandler(0x11, length: 17, ingame: true, extCmd: true)]
    protected static void Received_StatueAnimation(NetState ns, PacketReader pvSrc)
    {
        var e = new StatueAnimationEventArgs(ns);
        pvSrc.Seek(3, SeekOrigin.Current);
        //pvSrc.ReadInt16();  //  0x19
        //pvSrc.ReadByte();   //  0x05
        e.Mobile = pvSrc.ReadInt32();
        pvSrc.Seek(2, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        //pvSrc.ReadByte();   //  0xFF
        e.Status = pvSrc.ReadByte();
        pvSrc.Seek(1, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        e.Animation = pvSrc.ReadByte();
        pvSrc.Seek(1, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        e.Frame = pvSrc.ReadByte();
        OnStatueAnimation?.Invoke(e);
    }
}
