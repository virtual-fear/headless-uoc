namespace Client.Networking.Arguments;
using Client.Game;
public sealed record SkillInfo(int SkillID, ushort UV, ushort Base, byte Locked, ushort Cap);
public sealed class SkillUpdateEventArgs : EventArgs
{
    [PacketHandler(0x3A, length: -1, ingame: true)]
    private static event PacketEventHandler<SkillUpdateEventArgs>? Update;
    public NetState State { get; }
    public byte Type { get; }
    public List<SkillInfo> Skills { get; }
    private SkillUpdateEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Type = ip.ReadByte(); // 0x02    : type - absolute, capped
        Skills = new List<SkillInfo>();
        while (ip.ReadInt16() != 0)
        {
            ip.Seek(-2, SeekOrigin.Current);
            Skills.Add(new SkillInfo(
                SkillID: ip.ReadUInt16(),
                UV: ip.ReadUInt16(),
                Base: ip.ReadUInt16(),
                Locked: ip.ReadByte(),
                Cap: ip.ReadUInt16()));
        }
    }

    static SkillUpdateEventArgs() => Update += SkillUpdateEventArgs_Update;
    private static void SkillUpdateEventArgs_Update(SkillUpdateEventArgs e) => Player.OnSkillUpdate(e.State, e.Type, e.Skills);
}