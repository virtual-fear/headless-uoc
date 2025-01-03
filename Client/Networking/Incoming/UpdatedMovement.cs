using System;
using Client.Game;

namespace Client.Networking.Incoming
{
    using static PacketSink;
    public partial class PacketSink
    {
        #region EventArgs

        public sealed class MobileMovingEventArgs : EventArgs
        {
            public NetState State { get; }
            public MobileMovingEventArgs(NetState state) => State = state;
            public int Serial { get; set; }
            public short Body { get; set; }
            public short X { get; set; }
            public short Y { get; set; }
            public sbyte Z { get; set; }
            public Direction Direction { get; set; }
            public short Hue { get; set; }
            public byte PacketFlags { get; set; }
            public Notoriety Notoriety { get; set; }
        }
        public sealed class MovementAckEventArgs : EventArgs
        {
            public NetState State { get; }
            public MovementAckEventArgs(NetState state) => State = state;
            public byte Sequence { get; set; }
            public Notoriety Notoriety { get; set; }
        }
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

        #endregion (done)

        public static event PacketEventHandler<MobileMovingEventArgs> MobileMoving;
        public static event PacketEventHandler<MovementAckEventArgs> MovementAck;
        public static event PacketEventHandler<MovementRejEventArgs> MovementRej;
        public static void InvokeMovementRej(MovementRejEventArgs e) => MovementRej?.Invoke(e);
        public static void InvokeMovementAck(MovementAckEventArgs e) => MovementAck?.Invoke(e);
        public static void InvokeMobileMoving(MobileMovingEventArgs e) => MobileMoving?.Invoke(e);
    }
    public static class UpdatedMovement
    {
        public static void Configure()
        {
            Register(0x77, 17, true, new OnPacketReceive(MobileMoving));
            Register(0x22, 03, true, new OnPacketReceive(MovementAck));
            Register(0x21, 08, true, new OnPacketReceive(MovementRej));
        }

        private static void MovementRej(NetState ns, PacketReader pvSrc)
        {
            MovementRejEventArgs e = new MovementRejEventArgs(ns);

            e.Sequence = pvSrc.ReadByte();
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Direction = (Direction)pvSrc.ReadByte();
            e.Z = pvSrc.ReadSByte();

            PacketSink.InvokeMovementRej(e);
        }

        private static void MovementAck(NetState ns, PacketReader pvSrc)
        {
            MovementAckEventArgs e = new MovementAckEventArgs(ns);

            e.Sequence = pvSrc.ReadByte();
            e.Notoriety = (Notoriety)pvSrc.ReadByte();

            PacketSink.InvokeMovementAck(e);
        }

        private static void MobileMoving(NetState ns, PacketReader pvSrc)
        {
            MobileMovingEventArgs e = new MobileMovingEventArgs(ns);
            e.Serial = pvSrc.ReadInt32();
            e.Body = pvSrc.ReadInt16();
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Z = pvSrc.ReadSByte();
            e.Direction = (Direction)pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.PacketFlags = pvSrc.ReadByte();
            e.Notoriety = (Notoriety)pvSrc.ReadByte();

            PacketSink.InvokeMobileMoving(e);
        }

        private static void Register(int packetID, int length, bool ingame, OnPacketReceive receive)
        {
            PacketHandlers.Register(packetID, length, ingame, receive);
        }
    }
}
