using Direction = Client.Game.Data.Direction;
namespace Client.Networking.Arguments;
public sealed class MovementRejEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Sequence { get; }
    public short X { get; }
    public short Y { get; }
    public Direction Direction { get; }
    public sbyte Z { get; }
    internal MovementRejEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Sequence = ip.ReadByte();
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
        Direction = (Direction)ip.ReadByte();
        Z = ip.ReadSByte();
    }
}