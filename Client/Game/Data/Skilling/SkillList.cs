namespace Client.Game.Data;
public sealed class SkillList
{
    public Mobile Agent { get; }
    public Skill[] Array { get; } = new Skill[SkillInfo.Table.Length];
    public int Capacity { get; set; } = 0x1B58; // 7000
    public int Length => Array.Length;
    internal SkillList(Mobile src) => Agent = src;
    public Skill? this[SkillName name] => this[skillID: (int)name];
    public Skill? this[int skillID]
    {
        get
        {
            if (skillID < 0 || skillID >= Array.Length)
                return null;

            Skill sk = Array[skillID];

            if (sk == null)
                Array[skillID] = sk = new Skill(this, SkillInfo.Table[skillID], 0, 1000, SkillLock.Up);

            return sk;
        }
    }
}