namespace Client.Networking.Incoming;
public sealed class ServerChangeEventArgs : EventArgs
{
    public NetState State { get; }
    public ServerChangeEventArgs(NetState state) => State = state;
    public short X { get; set; }
    public short Y { get; set; }
    public short Z { get; set; }
    public short XMap { get; set; }
    public short YMap { get; set; }
}
public partial class Player
{
    public static event PacketEventHandler<ServerChangeEventArgs>? OnServerChange;

    [PacketHandler(0x76, length: 16, ingame: true)]
    protected static void Receive_ServerChange(NetState ns, PacketReader pvSrc)
    {
        ServerChangeEventArgs e = new(ns);
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadInt16();
        pvSrc.Seek(5, SeekOrigin.Current);
        e.XMap = pvSrc.ReadInt16();
        e.YMap = pvSrc.ReadInt16();
        OnServerChange?.Invoke(e);
    }
}