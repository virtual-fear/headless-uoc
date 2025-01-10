namespace Client.Networking.Incoming.Movement;

using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MobileMovingEventArgs>? OnMobileMoving;
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
    protected static class MobileMoving
    {
        [PacketHandler(0x77, length: 17, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            MobileMovingEventArgs e = new MobileMovingEventArgs(ns);
            e.Serial = (Serial)pvSrc.ReadUInt32();
            e.Body = pvSrc.ReadInt16();
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Z = pvSrc.ReadSByte();
            e.Direction = (Direction)pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.PacketFlags = pvSrc.ReadByte();
            e.Notoriety = (Notoriety)pvSrc.ReadByte();
            OnMobileMoving?.Invoke(e);
        }
    }
}