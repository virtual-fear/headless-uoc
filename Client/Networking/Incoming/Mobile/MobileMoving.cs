namespace Client.Networking.Incoming;
using Client.Game.Data;
using Client.Networking;
public sealed class MobileMovingEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileMovingEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
    public short Body { get; set; }
    public short X { get; set; }
    public short Y { get; set; }
    public sbyte Z { get; set; }
    public Direction Direction { get; set; }
    public short Hue { get; set; }
    public byte PacketFlags { get; set; }
    public Notoriety Notoriety { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<MobileMovingEventArgs>? OnMoving;

    [PacketHandler(0x77, length: 17, ingame: true)]
    protected static void Received_MobileMoving(NetState ns, PacketReader pvSrc)
    {
        MobileMovingEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        e.Body = pvSrc.ReadInt16();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadSByte();
        e.Direction = (Direction)pvSrc.ReadByte();
        e.Hue = pvSrc.ReadInt16();
        e.PacketFlags = pvSrc.ReadByte();
        e.Notoriety = (Notoriety)pvSrc.ReadByte();
        OnMoving?.Invoke(e);
    }
}