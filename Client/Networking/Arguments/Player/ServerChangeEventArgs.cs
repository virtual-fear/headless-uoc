namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class ServerChangeEventArgs : EventArgs
{
    [PacketHandler(0x76, length: 16, ingame: true)]
    private static event PacketEventHandler<ServerChangeEventArgs>? Update;
    public NetState State { get; }
    public short X { get; set; }
    public short Y { get; set; }
    public short Z { get; set; }
    public IPoint3D Location { get; }
    public short XMap { get; set; }
    public short YMap { get; set; }
    private ServerChangeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        X = ip.ReadInt16();
        Y = ip.ReadInt16();
        Z = ip.ReadInt16();
        Location = new Point3D()
        {
            X = X,
            Y = Y,
            Z = (sbyte)Z
        };
        ip.Seek(5, SeekOrigin.Current);
        XMap = ip.ReadInt16();
        YMap = ip.ReadInt16();
    }

    static ServerChangeEventArgs() => Update += ServerChangeEventArgs_Update;
    private static void ServerChangeEventArgs_Update(ServerChangeEventArgs e) => Player.OnServerChange(e.State, e.Location, e.XMap, e.YMap);
}