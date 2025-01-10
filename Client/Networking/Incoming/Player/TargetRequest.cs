namespace Client.Networking.Incoming;
using Client.Game.Data;
public class TargetReqEventArgs : EventArgs
{
    public NetState State { get; }
    public TargetReqEventArgs(NetState state) => State = state;
    public bool AllowGround { get; set; }
    public int TargetID { get; set; }
    public TargetFlags Flags { get; set; }
}

public partial class Player
{
    public static event PacketEventHandler<TargetReqEventArgs>? OnTargetRequest;

    [PacketHandler(0x6C, length: 19, ingame: true)]
    protected static void Receive_TargetRequest(NetState ns, PacketReader pvSrc)
    {
        TargetReqEventArgs e = new(ns);
        e.AllowGround = pvSrc.ReadBoolean();
        e.TargetID = pvSrc.ReadInt32();
        e.Flags = (TargetFlags)pvSrc.ReadByte();
        pvSrc.ReadBytes(13);
        OnTargetRequest?.Invoke(e);
    }
}
