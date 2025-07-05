namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MovementRejEventArgs : EventArgs
{
    [PacketHandler(0x21, length: 8, ingame: true)]
    private static event PacketEventHandler<MovementRejEventArgs>? Update;
    public NetState State { get; }
    public byte Sequence { get; }
    public Direction Direction { get; }
    public IPoint3D Location { get; }
    private MovementRejEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Sequence = ip.ReadByte();
        var x = ip.ReadInt16();
        var y = ip.ReadInt16();
        Direction = (Direction)ip.ReadByte();
        var z = ip.ReadSByte();
        Location = new Point3D() { X = x, Y = y, Z = z };
    }

    static MovementRejEventArgs() => Update += MovementRejEventArgs_Update;
    private static void MovementRejEventArgs_Update(MovementRejEventArgs e)
        => Mobile.OnMovementRej(e.State, e.Sequence, e.Direction, e.Location);
}