namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MultiTargetEventArgs : EventArgs
{
    [PacketHandler(0x99, length: 26, ingame: true)]
    private static event PacketEventHandler<MultiTargetEventArgs>? Update;
    public NetState State { get; }
    public bool AllowGround { get; }
    public int TargetID { get; }
    public TargetFlags Flags { get; }
    public short MultiID { get; }
    public short OffsetX { get; }
    public short OffsetY { get; }
    public short OffsetZ { get; }
    private MultiTargetEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        AllowGround = ip.ReadBoolean();
        TargetID = ip.ReadInt32();
        Flags = (TargetFlags)ip.ReadByte();
        ip.Seek(18, SeekOrigin.Begin);
        MultiID = ip.ReadInt16();
        OffsetX = ip.ReadInt16();
        OffsetY = ip.ReadInt16();
        OffsetZ = ip.ReadInt16();
    }

    static MultiTargetEventArgs() => Update += MultiTargetEventArgs_Update;
    private static void MultiTargetEventArgs_Update(MultiTargetEventArgs e) => Player.OnMultiTarget(e);
}