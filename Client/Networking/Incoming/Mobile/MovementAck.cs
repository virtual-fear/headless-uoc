namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MovementAckEventArgs : EventArgs
{
    public NetState State { get; }
    public MovementAckEventArgs(NetState state) => State = state;
    public byte Sequence { get; set; }
    public Notoriety Notoriety { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<MovementAckEventArgs>? OnMovementAck;

    [PacketHandler(0x22, length: 3, ingame: true)]
    protected static void Received_MovementAck(NetState ns, PacketReader pvSrc)
    {
        MovementAckEventArgs e = new(ns);
        e.Sequence = pvSrc.ReadByte();
        e.Notoriety = (Notoriety)pvSrc.ReadByte();
        OnMovementAck?.Invoke(e);
    }
}