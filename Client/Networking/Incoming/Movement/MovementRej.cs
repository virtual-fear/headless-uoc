namespace Client.Networking.Incoming.Movement;
using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MovementRejEventArgs>? OnMovementRej;
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
    protected static class MovementRej
    {
        [PacketHandler(0x21, length: 8, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
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
}