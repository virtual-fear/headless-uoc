namespace Client.Networking.Incoming;
public sealed record SkillInfo(int SkillID, ushort UV, ushort Base, byte Locked, ushort Cap);
public sealed class SkillUpdateEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Type { get; }
    public List<SkillInfo> Skills { get; }
    internal SkillUpdateEventArgs(NetState state, PacketReader ip)
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
}