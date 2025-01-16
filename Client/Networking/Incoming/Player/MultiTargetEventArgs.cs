using TargetFlags = Client.Game.Data.TargetFlags;
namespace Client.Networking.Incoming;
public sealed class MultiTargetEventArgs : EventArgs
{
    public NetState State { get; }
    public bool AllowGround { get; }
    public int TargetID { get; }
    public TargetFlags Flags { get; }
    public short MultiID { get; }
    public short OffsetX { get; }
    public short OffsetY { get; }
    public short OffsetZ { get; }
    internal MultiTargetEventArgs(NetState state, PacketReader ip)
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
}