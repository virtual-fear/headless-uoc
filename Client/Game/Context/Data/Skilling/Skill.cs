namespace Client.Game.Context.Data.Skilling;
public sealed class Skill
{
    public SkillList Owner { get; }
    internal SkillInfo Info { get; }
    public ushort Base { get; }
    public ushort Cap { get; } = 100; // default
    //public float Value { get; internal set; }
    //public float Real { get; internal set; }
    public SkillLock Lock { get; internal set; }
    public Skill(SkillList owner, SkillInfo info, int baseValue, int cap, SkillLock skillLock)
    {
        Owner = owner;
        Info = info;
        Base = (ushort)baseValue;
        Cap = (ushort)cap;
        Lock = skillLock;
    }
    public int Index => Info.SkillID;
    public SkillName Name => (SkillName)Index;
}
