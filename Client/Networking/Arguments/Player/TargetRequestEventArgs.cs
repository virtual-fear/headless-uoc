using TargetFlags = Client.Game.Data.TargetFlags;
namespace Client.Networking.Arguments;
public class TargetReqEventArgs : EventArgs
{
    public NetState State { get; }
    public bool AllowGround { get; set; }
    public int TargetID { get; set; }
    public TargetFlags Flags { get; set; }
    internal TargetReqEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        AllowGround = ip.ReadBoolean();
        TargetID = ip.ReadInt32();
        Flags = (TargetFlags)ip.ReadByte();
        ip.ReadBytes(13);
    }
}