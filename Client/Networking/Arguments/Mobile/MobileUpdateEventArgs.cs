using Serial = Client.Game.Data.Serial;
using Direction = Client.Game.Data.Direction;
namespace Client.Networking.Arguments;
public sealed class MobileUpdateEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; }
    public short Body { get; }
    public short Hue { get; }
    public byte PacketFlags { get; }
    public short X { get; }
    public short Y { get; }
    public Direction Direction { get; }
    public sbyte Z { get; }
    internal MobileUpdateEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        Body = (short)ip.ReadUInt16();
        ip.ReadByte();   //  0
        Hue = ip.ReadInt16();
        PacketFlags = ip.ReadByte();
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
        ip.ReadInt16();  //  0
        Direction = (Direction)ip.ReadByte();
        Z = ip.ReadSByte();
    }
}