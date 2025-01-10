namespace Client.Networking.Incoming.Shard;
using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<LoginConfirmEventArgs>? Shard_LoginConfirm;
    public sealed class LoginConfirmEventArgs : EventArgs
    {
        public NetState State { get; }
        public LoginConfirmEventArgs(NetState state) => State = state;
        public int Serial { get; set; } = -1;
        public short Body { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        public Direction Direction { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
    }
    protected static class LoginConfirm
    {
        [PacketHandler(0x1B, length: 37, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            LoginConfirmEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            pvSrc.Seek(4, SeekOrigin.Current);
            e.Body = pvSrc.ReadInt16();
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Z = pvSrc.ReadInt16();
            e.Direction = (Direction)pvSrc.ReadInt16();
            pvSrc.Seek(9, SeekOrigin.Current);
            e.Width = pvSrc.ReadInt16();
            e.Height = pvSrc.ReadInt16();
            Shard_LoginConfirm?.Invoke(e);
        }
    }
}