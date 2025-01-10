using Client.Game.Data;

namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MultiTargetEventArgs>? Player_MultiTarget;
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

    protected static class MultiTarget
    {
        [PacketHandler(0x99, length: 26, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
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
            Player_MultiTarget?.Invoke(e);
        }
    }
}