namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ServerChangeEventArgs>? Player_ServerChange;
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
    protected static class ServerChange
    {
        [PacketHandler(0x76, length: 16, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            ServerChangeEventArgs e = new(ns);
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Z = pvSrc.ReadInt16();
            pvSrc.Seek(5, SeekOrigin.Current);
            e.XMap = pvSrc.ReadInt16();
            e.YMap = pvSrc.ReadInt16();
            Player_ServerChange?.Invoke(e);
        }
    }
}
