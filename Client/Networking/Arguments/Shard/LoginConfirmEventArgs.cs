using Serial = Client.Game.Data.Serial;
using Direction = Client.Game.Data.Direction;
using Client.Game.Data;
using Client.Game;
namespace Client.Networking.Arguments;
public sealed class LoginConfirmEventArgs : EventArgs
{
    [PacketHandler(0x1B, length: 37, ingame: false)]
    internal static event PacketEventHandler<LoginConfirmEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public short Body { get; }
    public IPoint3D Location { get; }
    public Direction Direction { get; }
    public short Width { get; }
    public short Height { get; }
    internal LoginConfirmEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        ip.Seek(4, SeekOrigin.Current);
        Body = ip.ReadInt16();
        Location = new Point3D()
        {
            X = ip.ReadInt16(),
            Y = ip.ReadInt16(),
            Z = (sbyte)ip.ReadInt16()
        };
        Direction = (Direction)ip.ReadInt16();
        ip.Seek(9, SeekOrigin.Current);
        Width = ip.ReadInt16();
        Height = ip.ReadInt16();
    }
    static LoginConfirmEventArgs() => Update += LoginConfirmEventArgs_Update;
    private static void LoginConfirmEventArgs_Update(LoginConfirmEventArgs e) => World.WantMobile(e.Serial).Update(e);
}