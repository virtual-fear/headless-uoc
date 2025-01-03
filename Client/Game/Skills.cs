using System;

namespace Client.Game
{
    public delegate TimeSpan SkillUseCallback(Mobile m);

    #region Enums

    public enum SkillLock : byte
    {
        Up = 0,
        Down = 1,
        Locked = 2
    }

    public enum SkillName
    {
        Alchemy = 0,
        Anatomy = 1,
        AnimalLore = 2,
        ItemID = 3,
        ArmsLore = 4,
        Parry = 5,
        Begging = 6,
        Blacksmith = 7,
        Fletching = 8,
        Peacemaking = 9,
        Camping = 10,
        Carpentry = 11,
        Cartography = 12,
        Cooking = 13,
        DetectHidden = 14,
        Discordance = 15,
        EvalInt = 16,
        Healing = 17,
        Fishing = 18,
        Forensics = 19,
        Herding = 20,
        Hiding = 21,
        Provocation = 22,
        Inscribe = 23,
        Lockpicking = 24,
        Magery = 25,
        MagicResist = 26,
        Tactics = 27,
        Snooping = 28,
        Musicianship = 29,
        Poisoning = 30,
        Archery = 31,
        SpiritSpeak = 32,
        Stealing = 33,
        Tailoring = 34,
        AnimalTaming = 35,
        TasteID = 36,
        Tinkering = 37,
        Tracking = 38,
        Veterinary = 39,
        Swords = 40,
        Macing = 41,
        Fencing = 42,
        Wrestling = 43,
        Lumberjacking = 44,
        Mining = 45,
        Meditation = 46,
        Stealth = 47,
        RemoveTrap = 48,
        Necromancy = 49,
        Focus = 50,
        Chivalry = 51,
        Bushido = 52,
        Ninjitsu = 53,
        Spellweaving = 54,
        Mysticism = 55,
        Imbuing = 56,
        Throwing = 57
    }

    #endregion

    public class SkillInfo
    {
        private int m_SkillID;
        private string m_Name;
        private string m_Title;
        private double m_StrScale;
        private double m_DexScale;
        private double m_IntScale;
        private double m_StatTotal;
        private SkillUseCallback m_Callback;
        private double m_StrGain;
        private double m_DexGain;
        private double m_IntGain;
        private double m_GainFactor;

        public SkillInfo(int skillID, string name, double strScale, double dexScale, double intScale, string title, SkillUseCallback callback, double strGain, double dexGain, double intGain, double gainFactor)
        {
            m_Name = name;
            m_Title = title;
            m_SkillID = skillID;
            m_StrScale = strScale / 100.0;
            m_DexScale = dexScale / 100.0;
            m_IntScale = intScale / 100.0;
            m_Callback = callback;
            m_StrGain = strGain;
            m_DexGain = dexGain;
            m_IntGain = intGain;
            m_GainFactor = gainFactor;

            m_StatTotal = strScale + dexScale + intScale;
        }

        public SkillUseCallback Callback
        {
            get
            {
                return m_Callback;
            }
            set
            {
                m_Callback = value;
            }
        }

        public int SkillID
        {
            get
            {
                return m_SkillID;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                m_Title = value;
            }
        }

        public double StrScale
        {
            get
            {
                return m_StrScale;
            }
            set
            {
                m_StrScale = value;
            }
        }

        public double DexScale
        {
            get
            {
                return m_DexScale;
            }
            set
            {
                m_DexScale = value;
            }
        }

        public double IntScale
        {
            get
            {
                return m_IntScale;
            }
            set
            {
                m_IntScale = value;
            }
        }

        public double StatTotal
        {
            get
            {
                return m_StatTotal;
            }
            set
            {
                m_StatTotal = value;
            }
        }

        public double StrGain
        {
            get
            {
                return m_StrGain;
            }
            set
            {
                m_StrGain = value;
            }
        }

        public double DexGain
        {
            get
            {
                return m_DexGain;
            }
            set
            {
                m_DexGain = value;
            }
        }

        public double IntGain
        {
            get
            {
                return m_IntGain;
            }
            set
            {
                m_IntGain = value;
            }
        }

        public double GainFactor
        {
            get
            {
                return m_GainFactor;
            }
            set
            {
                m_GainFactor = value;
            }
        }

        private static SkillInfo[] m_Table = new SkillInfo[58]
			{
				new SkillInfo(  0, "Alchemy",			0.0,	5.0,	5.0,	"Alchemist",	null,	0.0,	0.5,	0.5,	1.0 ),
				new SkillInfo(  1, "Anatomy",			0.0,	0.0,	0.0,	"Biologist",	null,	0.15,	0.15,	0.7,	1.0 ),
				new SkillInfo(  2, "Animal Lore",		0.0,	0.0,	0.0,	"Naturalist",	null,	0.0,	0.0,	1.0,	1.0 ),
				new SkillInfo(  3, "Item Identification",	0.0,	0.0,	0.0,	"Merchant",	null,	0.0,	0.0,	1.0,	1.0 ),
				new SkillInfo(  4, "Arms Lore",			0.0,	0.0,	0.0,	"Weapon Master",	null,	0.75,	0.15,	0.1,	1.0 ),
				new SkillInfo(  5, "Parrying",			7.5,	2.5,	0.0,	"Duelist",	null,	0.75,	0.25,	0.0,	1.0 ),
				new SkillInfo(  6, "Begging",			0.0,	0.0,	0.0,	"Beggar",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo(  7, "Blacksmithy",		10.0,	0.0,	0.0,	"Blacksmith",	null,	1.0,	0.0,	0.0,	1.0 ),
				new SkillInfo(  8, "Bowcraft/Fletching",	6.0,	16.0,	0.0,	"Bowyer",	null,	0.6,	1.6,	0.0,	1.0 ),
				new SkillInfo(  9, "Peacemaking",		0.0,	0.0,	0.0,	"Pacifier",		null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 10, "Camping",			20.0,	15.0,	15.0,	"Explorer",	null,	2.0,	1.5,	1.5,	1.0 ),
				new SkillInfo( 11, "Carpentry",			20.0,	5.0,	0.0,	"Carpenter",	null,	2.0,	0.5,	0.0,	1.0 ),
				new SkillInfo( 12, "Cartography",		0.0,	7.5,	7.5,	"Cartographer",	null,	0.0,	0.75,	0.75,	1.0 ),
				new SkillInfo( 13, "Cooking",			0.0,	20.0,	30.0,	"Chef",		null,	0.0,	2.0,	3.0,	1.0 ),
				new SkillInfo( 14, "Detecting Hidden",		0.0,	0.0,	0.0,	"Scout",	null,	0.0,	0.4,	0.6,	1.0 ),
				new SkillInfo( 15, "Discordance",		0.0,	2.5,	2.5,	"Demoralizer",		null,	0.0,	0.25,	0.25,	1.0 ),
				new SkillInfo( 16, "Evaluating Intelligence",	0.0,	0.0,	0.0,	"Scholar",	null,	0.0,	0.0,	1.0,	1.0 ),
				new SkillInfo( 17, "Healing",			6.0,	6.0,	8.0,	"Healer",	null,	0.6,	0.6,	0.8,	1.0 ),
				new SkillInfo( 18, "Fishing",			0.0,	0.0,	0.0,	"Fisherman",	null,	0.5,	0.5,	0.0,	1.0 ),
				new SkillInfo( 19, "Forensic Evaluation",	0.0,	0.0,	0.0,	"Detective",	null,	0.0,	0.2,	0.8,	1.0 ),
				new SkillInfo( 20, "Herding",			16.25,	6.25,	2.5,	"Shepherd",	null,	1.625,	0.625,	0.25,	1.0 ),
				new SkillInfo( 21, "Hiding",			0.0,	0.0,	0.0,	"Shade",	null,	0.0,	0.8,	0.2,	1.0 ),
				new SkillInfo( 22, "Provocation",		0.0,	4.5,	0.5,	"Rouser",		null,	0.0,	0.45,	0.05,	1.0 ),
				new SkillInfo( 23, "Inscription",		0.0,	2.0,	8.0,	"Scribe",	null,	0.0,	0.2,	0.8,	1.0 ),
				new SkillInfo( 24, "Lockpicking",		0.0,	25.0,	0.0,	"Infiltrator",	null,	0.0,	2.0,	0.0,	1.0 ),
				new SkillInfo( 25, "Magery",			0.0,	0.0,	15.0,	"Mage",		null,	0.0,	0.0,	1.5,	1.0 ),
				new SkillInfo( 26, "Resisting Spells",		0.0,	0.0,	0.0,	"Warder",		null,	0.25,	0.25,	0.5,	1.0 ),
				new SkillInfo( 27, "Tactics",			0.0,	0.0,	0.0,	"Tactician",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 28, "Snooping",			0.0,	25.0,	0.0,	"Spy",	null,	0.0,	2.5,	0.0,	1.0 ),
				new SkillInfo( 29, "Musicianship",		0.0,	0.0,	0.0,	"Bard",		null,	0.0,	0.8,	0.2,	1.0 ),
				new SkillInfo( 30, "Poisoning",			0.0,	4.0,	16.0,	"Assassin",	null,	0.0,	0.4,	1.6,	1.0 ),
				new SkillInfo( 31, "Archery",			2.5,	7.5,	0.0,	"Archer",	null,	0.25,	0.75,	0.0,	1.0 ),
				new SkillInfo( 32, "Spirit Speak",		0.0,	0.0,	0.0,	"Medium",	null,	0.0,	0.0,	1.0,	1.0 ),
				new SkillInfo( 33, "Stealing",			0.0,	10.0,	0.0,	"Pickpocket",	null,	0.0,	1.0,	0.0,	1.0 ),
				new SkillInfo( 34, "Tailoring",			3.75,	16.25,	5.0,	"Tailor",	null,	0.38,	1.63,	0.5,	1.0 ),
				new SkillInfo( 35, "Animal Taming",		14.0,	2.0,	4.0,	"Tamer",	null,	1.4,	0.2,	0.4,	1.0 ),
				new SkillInfo( 36, "Taste Identification",	0.0,	0.0,	0.0,	"Praegustator",		null,	0.2,	0.0,	0.8,	1.0 ),
				new SkillInfo( 37, "Tinkering",			5.0,	2.0,	3.0,	"Tinker",	null,	0.5,	0.2,	0.3,	1.0 ),
				new SkillInfo( 38, "Tracking",			0.0,	12.5,	12.5,	"Ranger",	null,	0.0,	1.25,	1.25,	1.0 ),
				new SkillInfo( 39, "Veterinary",		8.0,	4.0,	8.0,	"Veterinarian",	null,	0.8,	0.4,	0.8,	1.0 ),
				new SkillInfo( 40, "Swordsmanship",		7.5,	2.5,	0.0,	"Swordsman",	null,	0.75,	0.25,	0.0,	1.0 ),
				new SkillInfo( 41, "Mace Fighting",		9.0,	1.0,	0.0,	"Armsman",	null,	0.9,	0.1,	0.0,	1.0 ),
				new SkillInfo( 42, "Fencing",			4.5,	5.5,	0.0,	"Fencer",	null,	0.45,	0.55,	0.0,	1.0 ),
				new SkillInfo( 43, "Wrestling",			9.0,	1.0,	0.0,	"Wrestler",	null,	0.9,	0.1,	0.0,	1.0 ),
				new SkillInfo( 44, "Lumberjacking",		20.0,	0.0,	0.0,	"Lumberjack",	null,	2.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 45, "Mining",			20.0,	0.0,	0.0,	"Miner",	null,	2.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 46, "Meditation",		0.0,	0.0,	0.0,	"Stoic",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 47, "Stealth",			0.0,	0.0,	0.0,	"Rogue",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 48, "Remove Trap",		0.0,	0.0,	0.0,	"Trap Specialist",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 49, "Necromancy",		0.0,	0.0,	0.0,	"Necromancer",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 50, "Focus",			0.0,	0.0,	0.0,	"Driven",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 51, "Chivalry",			0.0,	0.0,	0.0,	"Paladin",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 52, "Bushido",			0.0,	0.0,	0.0,	"Samurai",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 53, "Ninjitsu",			0.0,	0.0,	0.0,	"Ninja",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 54, "Spellweaving",		0.0,	0.0,	0.0,	"Arcanist",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 55, "Mysticism",			0.0,	0.0,	0.0,	"Mystic",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 56, "Imbuing",			0.0,	0.0,	0.0,	"Artificer",	null,	0.0,	0.0,	0.0,	1.0 ),
				new SkillInfo( 57, "Throwing",			0.0,	0.0,	0.0,	"Bladeweaver",	null,	0.0,	0.0,	0.0,	1.0 ),
			};

        public static SkillInfo[] Table
        {
            get
            {
                return m_Table;
            }
            set
            {
                m_Table = value;
            }
        }
    }

    public class Skill
    {
        private Skills m_Owner;
        private SkillInfo m_Info;
        private ushort m_Base;
        private ushort m_Cap;
        private SkillLock m_Lock;

        public Skill(Skills owner, SkillInfo info, int baseValue, int cap, SkillLock skillLock)
        {
            m_Owner = owner;
            m_Info = info;
            m_Base = (ushort)baseValue;
            m_Cap = (ushort)cap;
            m_Lock = skillLock;
        }

        public Skills Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public SkillName SkillName
        {
            get
            {
                return (SkillName)m_Info.SkillID;
            }
        }

        public int SkillID
        {
            get
            {
                return m_Info.SkillID;
            }
        }

        public float Value { get; internal set; }
        public float Real { get; internal set; }
        public SkillLock Lock { get; internal set; }
    }

    public class Skills
    {
        private Mobile m_Owner;
        private int m_Cap;
        private Skill[] m_Skills;

        public int Cap
        {
            get { return m_Cap; }
            set { m_Cap = value; }
        }

        public Mobile Owner
        {
            get { return m_Owner; }
        }

        public int Length
        {
            get { return m_Skills.Length; }
        }

        public Skill this[SkillName name]
        {
            get { return this[(int)name]; }
        }

        public Skill this[int skillID]
        {
            get
            {
                if (skillID < 0 || skillID >= m_Skills.Length)
                    return null;

                Skill sk = m_Skills[skillID];

                if (sk == null)
                    m_Skills[skillID] = sk = new Skill(this, SkillInfo.Table[skillID], 0, 1000, SkillLock.Up);

                return sk;
            }
        }

        public Skills(Mobile owner)
        {
            m_Owner = owner;
            m_Cap = 7000;

            SkillInfo[] info = SkillInfo.Table;

            m_Skills = new Skill[info.Length];
        }
    }
}