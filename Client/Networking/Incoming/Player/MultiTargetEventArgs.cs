namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MultiTargetEventArgs : EventArgs
{
    public NetState State { get; }
    public MultiTargetEventArgs(NetState state) => State = state;
    public bool AllowGround { get; set; }
    public int TargetID { get; set; }
    public TargetFlags Flags { get; set; }
    public short MultiID { get; set; }
    public short OffsetX { get; set; }
    public short OffsetY { get; set; }
    public short OffsetZ { get; set; }
}

public partial class Player
{
    public static event PacketEventHandler<MultiTargetEventArgs>? OnMultiTarget;

    [PacketHandler(0x99, length: 26, ingame: true)]
    protected static void Receive_MultiTarget(NetState ns, PacketReader pvSrc)
    {
        MultiTargetEventArgs e = new(ns);
        e.AllowGround = pvSrc.ReadBoolean();
        e.TargetID = pvSrc.ReadInt32();
        e.Flags = (TargetFlags)pvSrc.ReadByte();
        pvSrc.Seek(18, SeekOrigin.Begin);
        e.MultiID = pvSrc.ReadInt16();
        e.OffsetX = pvSrc.ReadInt16();
        e.OffsetY = pvSrc.ReadInt16();
        e.OffsetZ = pvSrc.ReadInt16();
        OnMultiTarget?.Invoke(e);
    }
}