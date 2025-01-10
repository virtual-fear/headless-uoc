namespace Client.Networking.Incoming.Movement;
using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MovementAckEventArgs>? OnMovementAck;
    public sealed class MovementAckEventArgs : EventArgs
    {
        public NetState State { get; }
        public MovementAckEventArgs(NetState state) => State = state;
        public byte Sequence { get; set; }
        public Notoriety Notoriety { get; set; }
    }

    protected static class MovementAck
    {
        [PacketHandler(0x22, length: 3, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            MovementAckEventArgs e = new(ns);
            e.Sequence = pvSrc.ReadByte();
            e.Notoriety = (Notoriety)pvSrc.ReadByte();
            OnMovementAck?.Invoke(e);
        }
    }
}
