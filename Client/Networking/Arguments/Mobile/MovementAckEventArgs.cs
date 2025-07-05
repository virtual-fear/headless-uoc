namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MovementAckEventArgs : EventArgs
{
    [PacketHandler(0x22, length: 3, ingame: true)]
    private static event PacketEventHandler<MovementAckEventArgs>? Update;
    public NetState State { get; }
    public byte Sequence { get; }
    public Notoriety Notoriety { get; }
    private MovementAckEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Sequence = pvSrc.ReadByte();
        Notoriety = (Notoriety)pvSrc.ReadByte();
    }

    static MovementAckEventArgs() => Update += MovementAckEventArgs_Update;
    private static void MovementAckEventArgs_Update(MovementAckEventArgs e)
        => Mobile.OnMovementAck(e.State, e.Sequence, e.Notoriety);
}