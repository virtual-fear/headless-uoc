namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class TargetReqEventArgs : EventArgs
{
    [PacketHandler(0x6C, length: 19, ingame: true)]
    private static event PacketEventHandler<TargetReqEventArgs>? Update;
    public NetState State { get; }
    public bool AllowGround { get; set; }
    public int TargetID { get; set; }
    public TargetFlags Flags { get; set; }
    private TargetReqEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        AllowGround = ip.ReadBoolean();
        TargetID = ip.ReadInt32();
        Flags = (TargetFlags)ip.ReadByte();
        ip.ReadBytes(13);
    }
    static TargetReqEventArgs() => Update += TargetReqEventArgs_Update;
    private static void TargetReqEventArgs_Update(TargetReqEventArgs e)
        => Player.OnTarget(e.State, e.TargetID, e.Flags);
}