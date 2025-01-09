using Client.Game;

namespace Client.Game.Data.Skilling;
public delegate void SkillInfoCallback(SkillInfo info, Mobile from);
public sealed class SkillInfo
{
    public int SkillID { get; }
    public string Name { get; set; }
    public string Title { get; set; }
    public double StrScale { get; set; }
    public double DexScale { get; set; }
    public double IntScale { get; set; }
    public double StatTotal { get; set; }
    public SkillInfoCallback Callback { get; set; }
    public double StrGain { get; set; }
    public double DexGain { get; set; }
    public double IntGain { get; set; }
    public double GainFactor { get; set; }
    internal SkillInfo(int skillID, string name, double strScale, double dexScale, double intScale, string title,
                     SkillInfoCallback callback, double strGain, double dexGain, double intGain, double gainFactor)
    {
        SkillID = skillID;
        Name = name;
        Title = title;
        StrScale = strScale / 100.0;
        DexScale = dexScale / 100.0;
        IntScale = intScale / 100.0;
        Callback = callback;
        StrGain = strGain;
        DexGain = dexGain;
        IntGain = intGain;
        GainFactor = gainFactor;
        StatTotal = strScale + dexScale + intScale;
    }
    public static SkillInfo[] Table { get; set; } = new SkillInfo[58]
    {
        new SkillInfo(0, "Alchemy", 0.0, 5.0, 5.0, "Alchemist", null, 0.0, 0.5, 0.5, 1.0),
        new SkillInfo(1, "Anatomy", 0.0, 0.0, 0.0, "Biologist", null, 0.15, 0.15, 0.7, 1.0),
        new SkillInfo(2, "Animal Lore", 0.0, 0.0, 0.0, "Naturalist", null, 0.0, 0.0, 1.0, 1.0),
        new SkillInfo(3, "Item Identification", 0.0, 0.0, 0.0, "Merchant", null, 0.0, 0.0, 1.0, 1.0),
        new SkillInfo(4, "Arms Lore", 0.0, 0.0, 0.0, "Weapon Master", null, 0.75, 0.15, 0.1, 1.0),
        new SkillInfo(5, "Parrying", 7.5, 2.5, 0.0, "Duelist", null, 0.75, 0.25, 0.0, 1.0),
        new SkillInfo(6, "Begging", 0.0, 0.0, 0.0, "Beggar", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(7, "Blacksmithy", 10.0, 0.0, 0.0, "Blacksmith", null, 1.0, 0.0, 0.0, 1.0),
        new SkillInfo(8, "Bowcraft/Fletching", 6.0, 16.0, 0.0, "Bowyer", null, 0.6, 1.6, 0.0, 1.0),
        new SkillInfo(9, "Peacemaking", 0.0, 0.0, 0.0, "Pacifier", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(10, "Camping", 20.0, 15.0, 15.0, "Explorer", null, 2.0, 1.5, 1.5, 1.0),
        new SkillInfo(11, "Carpentry", 20.0, 5.0, 0.0, "Carpenter", null, 2.0, 0.5, 0.0, 1.0),
        new SkillInfo(12, "Cartography", 0.0, 7.5, 7.5, "Cartographer", null, 0.0, 0.75, 0.75, 1.0),
        new SkillInfo(13, "Cooking", 0.0, 20.0, 30.0, "Chef", null, 0.0, 2.0, 3.0, 1.0),
        new SkillInfo(14, "Detecting Hidden", 0.0, 0.0, 0.0, "Scout", null, 0.0, 0.4, 0.6, 1.0),
        new SkillInfo(15, "Discordance", 0.0, 2.5, 2.5, "Demoralizer", null, 0.0, 0.25, 0.25, 1.0),
        new SkillInfo(16, "Evaluating Intelligence", 0.0, 0.0, 0.0, "Scholar", null, 0.0, 0.0, 1.0, 1.0),
        new SkillInfo(17, "Healing", 6.0, 6.0, 8.0, "Healer", null, 0.6, 0.6, 0.8, 1.0),
        new SkillInfo(18, "Fishing", 0.0, 0.0, 0.0, "Fisherman", null, 0.5, 0.5, 0.0, 1.0),
        new SkillInfo(19, "Forensic Evaluation", 0.0, 0.0, 0.0, "Detective", null, 0.0, 0.2, 0.8, 1.0),
        new SkillInfo(20, "Herding", 16.25, 6.25, 2.5, "Shepherd", null, 1.625, 0.625, 0.25, 1.0),
        new SkillInfo(21, "Hiding", 0.0, 0.0, 0.0, "Shade", null, 0.0, 0.8, 0.2, 1.0),
        new SkillInfo(22, "Provocation", 0.0, 4.5, 0.5, "Rouser", null, 0.0, 0.45, 0.05, 1.0),
        new SkillInfo(23, "Inscription", 0.0, 2.0, 8.0, "Scribe", null, 0.0, 0.2, 0.8, 1.0),
        new SkillInfo(24, "Lockpicking", 0.0, 25.0, 0.0, "Infiltrator", null, 0.0, 2.0, 0.0, 1.0),
        new SkillInfo(25, "Magery", 0.0, 0.0, 15.0, "Mage", null, 0.0, 0.0, 1.5, 1.0),
        new SkillInfo(26, "Resisting Spells", 0.0, 0.0, 0.0, "Warder", null, 0.25, 0.25, 0.5, 1.0),
        new SkillInfo(27, "Tactics", 0.0, 0.0, 0.0, "Tactician", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(28, "Snooping", 0.0, 25.0, 0.0, "Spy", null, 0.0, 2.5, 0.0, 1.0),
        new SkillInfo(29, "Musicianship", 0.0, 0.0, 0.0, "Bard", null, 0.0, 0.8, 0.2, 1.0),
        new SkillInfo(30, "Poisoning", 0.0, 4.0, 16.0, "Assassin", null, 0.0, 0.4, 1.6, 1.0),
        new SkillInfo(31, "Archery", 2.5, 7.5, 0.0, "Archer", null, 0.25, 0.75, 0.0, 1.0),
        new SkillInfo(32, "Spirit Speak", 0.0, 0.0, 0.0, "Medium", null, 0.0, 0.0, 1.0, 1.0),
        new SkillInfo(33, "Stealing", 0.0, 10.0, 0.0, "Pickpocket", null, 0.0, 1.0, 0.0, 1.0),
        new SkillInfo(34, "Tailoring", 3.75, 16.25, 5.0, "Tailor", null, 0.38, 1.63, 0.5, 1.0),
        new SkillInfo(35, "Animal Taming", 14.0, 2.0, 4.0, "Tamer", null, 1.4, 0.2, 0.4, 1.0),
        new SkillInfo(36, "Taste Identification", 0.0, 0.0, 0.0, "Praegustator", null, 0.2, 0.0, 0.8, 1.0),
        new SkillInfo(37, "Tinkering", 5.0, 2.0, 3.0, "Tinker", null, 0.5, 0.2, 0.3, 1.0),
        new SkillInfo(38, "Tracking", 0.0, 12.5, 12.5, "Ranger", null, 0.0, 1.25, 1.25, 1.0),
        new SkillInfo(39, "Veterinary", 8.0, 4.0, 8.0, "Veterinarian", null, 0.8, 0.4, 0.8, 1.0),
        new SkillInfo(40, "Swordsmanship", 7.5, 2.5, 0.0, "Swordsman", null, 0.75, 0.25, 0.0, 1.0),
        new SkillInfo(41, "Mace Fighting", 9.0, 1.0, 0.0, "Armsman", null, 0.9, 0.1, 0.0, 1.0),
        new SkillInfo(42, "Fencing", 4.5, 5.5, 0.0, "Fencer", null, 0.45, 0.55, 0.0, 1.0),
        new SkillInfo(43, "Wrestling", 9.0, 1.0, 0.0, "Wrestler", null, 0.9, 0.1, 0.0, 1.0),
        new SkillInfo(44, "Lumberjacking", 20.0, 0.0, 0.0, "Lumberjack", null, 2.0, 0.0, 0.0, 1.0),
        new SkillInfo(45, "Mining", 20.0, 0.0, 0.0, "Miner", null, 2.0, 0.0, 0.0, 1.0),
        new SkillInfo(46, "Meditation", 0.0, 0.0, 0.0, "Stoic", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(47, "Stealth", 0.0, 0.0, 0.0, "Rogue", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(48, "Remove Trap", 0.0, 0.0, 0.0, "Trap Specialist", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(49, "Necromancy", 0.0, 0.0, 0.0, "Necromancer", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(50, "Focus", 0.0, 0.0, 0.0, "Driven", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(51, "Chivalry", 0.0, 0.0, 0.0, "Paladin", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(52, "Bushido", 0.0, 0.0, 0.0, "Samurai", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(53, "Ninjitsu", 0.0, 0.0, 0.0, "Ninja", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(54, "Spellweaving", 0.0, 0.0, 0.0, "Arcanist", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(55, "Mysticism", 0.0, 0.0, 0.0, "Mystic", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(56, "Imbuing", 0.0, 0.0, 0.0, "Artificer", null, 0.0, 0.0, 0.0, 1.0),
        new SkillInfo(57, "Throwing", 0.0, 0.0, 0.0, "Bladeweaver", null, 0.0, 0.0, 0.0, 1.0)
    };
}