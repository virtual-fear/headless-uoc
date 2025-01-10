namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<SkillUpdateEventArgs>? Player_SkillUpdate;
    public class SkillInfo
    {
        public int SkillID { get; }
        public ushort UV { get; }
        public ushort Base { get; }
        public byte Locked { get; }
        public ushort Cap { get; }
        private SkillInfo(PacketReader pvSrc)
        {
            SkillID = pvSrc.ReadUInt16();
            UV = pvSrc.ReadUInt16();
            Base = pvSrc.ReadUInt16();
            Locked = pvSrc.ReadByte();
            Cap = pvSrc.ReadUInt16();
        }
        public static SkillInfo Instantiate(PacketReader pvSrc) => new SkillInfo(pvSrc);
    }
    public sealed class SkillUpdateEventArgs : EventArgs
    {
        public NetState State { get; }
        public SkillUpdateEventArgs(NetState state) => State = state;
        public byte Type { get; set; }
        public List<SkillInfo> Skills { get; set; } = new List<SkillInfo>();
    }

    protected static class SkillUpdate
    {
        [PacketHandler(0x3A, length: -1, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            SkillUpdateEventArgs e = new(ns);
            e.Type = pvSrc.ReadByte();  //  0x02    :   type-- absolute, capped
            while (pvSrc.ReadInt16() != 0)
            {
                pvSrc.Seek(-2, SeekOrigin.Current);
                e.Skills.Add(SkillInfo.Instantiate(pvSrc));
            }
            Player_SkillUpdate?.Invoke(e);
        }
    }
}
