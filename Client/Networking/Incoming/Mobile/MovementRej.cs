namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MovementRejEventArgs : EventArgs
{
    public NetState State { get; }
    public MovementRejEventArgs(NetState state) => State = state;
    public byte Sequence { get; set; }
    public short X { get; set; }
    public short Y { get; set; }
    public Direction Direction { get; set; }
    public sbyte Z { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<MovementRejEventArgs>? OnMovementRej;

    [PacketHandler(0x21, length: 8, ingame: true)]
    protected static void Received_MovementRej(NetState ns, PacketReader pvSrc)
    {
        MovementRejEventArgs e = new(ns);
        e.Sequence = pvSrc.ReadByte();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Direction = (Direction)pvSrc.ReadByte();
        e.Z = pvSrc.ReadSByte();
        OnMovementRej?.Invoke(e);
    }
}