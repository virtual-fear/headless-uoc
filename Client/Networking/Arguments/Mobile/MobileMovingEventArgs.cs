using Serial = Client.Game.Data.Serial;
using Direction = Client.Game.Data.Direction;
using Notoriety = Client.Game.Data.Notoriety;
namespace Client.Networking.Arguments;
public sealed class MobileMovingEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; }
    public short Body { get; }
    public short X { get; }
    public short Y { get; }
    public sbyte Z { get; }
    public Direction Direction { get; }
    public short Hue { get; }
    public byte PacketFlags { get; }
    public Notoriety Notoriety { get; }
    internal MobileMovingEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        Body = ip.ReadInt16();
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
        Z = ip.ReadSByte();
        Direction = (Direction)ip.ReadByte();
        Hue = ip.ReadInt16();
        PacketFlags = ip.ReadByte();
        Notoriety = (Notoriety)ip.ReadByte();
    }
}