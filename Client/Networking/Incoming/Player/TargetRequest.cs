using Client.Game.Data;
namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<TargetReqEventArgs>? Player_TargetRequest;
    public class TargetReqEventArgs : EventArgs
    {
        public NetState State { get; }
        public TargetReqEventArgs(NetState state) => State = state;
        public bool AllowGround { get; set; }
        public int TargetID { get; set; }
        public TargetFlags Flags { get; set; }
    }
    protected static class TargetRequest
    {
        [PacketHandler(0x6C, length: 19, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            TargetReqEventArgs e = new(ns);
            e.AllowGround = pvSrc.ReadBoolean();
            e.TargetID = pvSrc.ReadInt32();
            e.Flags = (TargetFlags)pvSrc.ReadByte();
            pvSrc.ReadBytes(13);
            Player_TargetRequest?.Invoke(e);
        }
    }
}