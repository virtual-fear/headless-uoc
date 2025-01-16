using Serial = Client.Game.Data.Serial;
using Direction = Client.Game.Data.Direction;
namespace Client.Networking.Incoming;
public sealed class LoginConfirmEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; }
    public short Body { get; }
    public short X { get; }
    public short Y { get; }
    public short Z { get; }
    public Direction Direction { get; }
    public short Width { get; }
    public short Height { get; }
    internal LoginConfirmEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        ip.Seek(4, SeekOrigin.Current);
        Body = ip.ReadInt16();
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
        Z = ip.ReadInt16();
        Direction = (Direction)ip.ReadInt16();
        ip.Seek(9, SeekOrigin.Current);
        Width = ip.ReadInt16();
        Height = ip.ReadInt16();
    }
}