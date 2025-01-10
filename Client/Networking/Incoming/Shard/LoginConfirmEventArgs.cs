namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class LoginConfirmEventArgs : EventArgs
{
    public NetState State { get; }
    public LoginConfirmEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; } = (Serial)0;
    public short Body { get; set; }
    public short X { get; set; }
    public short Y { get; set; }
    public short Z { get; set; }
    public Direction Direction { get; set; }
    public short Width { get; set; }
    public short Height { get; set; }
}
public partial class Shard
{
    public static event PacketEventHandler<LoginConfirmEventArgs>? OnLoginConfirm;

    [PacketHandler(0x1B, length: 37, ingame: true)]
    protected static void Received_LoginConfirm(NetState ns, PacketReader pvSrc)
    {
        LoginConfirmEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        pvSrc.Seek(4, SeekOrigin.Current);
        e.Body = pvSrc.ReadInt16();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadInt16();
        e.Direction = (Direction)pvSrc.ReadInt16();
        pvSrc.Seek(9, SeekOrigin.Current);
        e.Width = pvSrc.ReadInt16();
        e.Height = pvSrc.ReadInt16();
        OnLoginConfirm?.Invoke(e);
    }
}