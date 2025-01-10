namespace Client.Networking.Incoming;
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

public partial class Player
{
    public static event PacketEventHandler<SkillUpdateEventArgs>? OnSkillUpdate;

    [PacketHandler(0x3A, length: -1, ingame: true)]
    protected static void Receive_SkillUpdate(NetState ns, PacketReader pvSrc)
    {
        SkillUpdateEventArgs e = new(ns);
        e.Type = pvSrc.ReadByte();  //  0x02    :   type-- absolute, capped
        while (pvSrc.ReadInt16() != 0)
        {
            pvSrc.Seek(-2, SeekOrigin.Current);
            e.Skills.Add(SkillInfo.Instantiate(pvSrc));
        }
        OnSkillUpdate?.Invoke(e);
    }
}