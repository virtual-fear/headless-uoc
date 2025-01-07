namespace Client.Game;
using Client.Game.Data.Mobiles;

public struct Body
{
    private static readonly string m_Path = @"data\bodytable.cfg";

    private static BodyType[] m_Types;

    private int m_BodyID;

    public int BodyID
    {
        get { return m_BodyID; }
    }

    public BodyType Type
    {
        get
        {
            if (m_BodyID >= 0 && m_BodyID < m_Types.Length)
                return m_Types[m_BodyID];
            else
                return BodyType.Empty;
        }
    }

    public bool IsHumanoid
    {
        get
        {
            return m_BodyID >= 0
                && m_BodyID < m_Types.Length
                && m_Types[m_BodyID] == BodyType.Human
                && m_BodyID != 402
                && m_BodyID != 403
                && m_BodyID != 607
                && m_BodyID != 608
                && m_BodyID != 694
                && m_BodyID != 695
                && m_BodyID != 970;
        }
    }

    public bool IsGargoyle
    {
        get
        {
            return m_BodyID == 666
                || m_BodyID == 667
                || m_BodyID == 694
                || m_BodyID == 695;
        }
    }

    public bool IsMale
    {
        get
        {
            return m_BodyID == 183
                || m_BodyID == 185
                || m_BodyID == 400
                || m_BodyID == 402
                || m_BodyID == 605
                || m_BodyID == 607
                || m_BodyID == 666
                || m_BodyID == 694
                || m_BodyID == 750;
        }
    }

    public bool IsFemale {
        get {
            switch ( m_BodyID ) {
                case 184:
                case 186:
                case 401:
                case 403:
                case 606:
                case 608:
                case 667:
                case 695:
                case 751:
                return true;
            }
            return false;
        }
    }

    public bool IsGhost
    {
        get
        {
            switch ( m_BodyID ) {
                case 402:
                case 403:
                case 607:
                case 608:
                case 694:
                case 695:
                case 970:
                return true;
            }
            return false;
        }
    }

    public bool IsMonster
    {
        get { return CheckBodyID( m_BodyID, ref m_Types, BodyType.Monster ); }
    }

    public bool IsAnimal
    {
        get { return CheckBodyID( m_BodyID, ref m_Types, BodyType.Animal ); }
    }

    public bool IsEmpty
    {
        get { return CheckBodyID( m_BodyID, ref m_Types, BodyType.Empty ); }
    }

    public bool IsSea
    {
        get { return CheckBodyID( m_BodyID, ref m_Types, BodyType.Sea ); }
    }

    public bool IsEquipment
    {
        get { throw new NotImplementedException(); }
    }

    static bool CheckBodyID( int bodyID, ref BodyType[] types, BodyType state ) {

        if ( bodyID < 0 )
            return false;

        if ( bodyID >= types.Length )
            throw new ArgumentOutOfRangeException( "bodyID", "Index out of range." );

        return types[bodyID] == state;
    }

    public Body(int bodyID)
    {
        m_BodyID = bodyID;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        else if (obj is Body)
            return ((Body)obj).BodyID == m_BodyID;
        else
            return false;
    }

    public override int GetHashCode()
    {
        return m_BodyID;
    }

    public override string ToString()
    {
        return string.Format("0x{0:X}", m_BodyID);
    }

    public static implicit operator int(Body b)
    {
        return b.BodyID;
    }

    public static implicit operator Body(int bodyID)
    {
        return new Body(bodyID);
    }

    public static bool operator ==(Body l, Body r)
    {
        return l.BodyID == r.BodyID;
    }

    public static bool operator !=(Body l, Body r)
    {
        return l.BodyID != r.BodyID;
    }

    public static bool operator >(Body l, Body r)
    {
        return l.BodyID > r.BodyID;
    }

    public static bool operator >=(Body l, Body r)
    {
        return l.BodyID >= r.BodyID;
    }

    public static bool operator <(Body l, Body r)
    {
        return l.BodyID < r.BodyID;
    }

    public static bool operator <=(Body l, Body r)
    {
        return l.BodyID <= r.BodyID;
    }

    static Body()
    {
        if (File.Exists(m_Path))
        {
            using (StreamReader ip = new StreamReader(m_Path))
            {
                m_Types = new BodyType[0x1000];

                string line;

                while ((line = ip.ReadLine()) != null)
                {
                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] split = line.Split('\t');

                    BodyType type;
                    int bodyID;

                    if (int.TryParse(split[0], out bodyID) && Enum.TryParse(split[1], true, out type) && bodyID >= 0 && bodyID < m_Types.Length)
                    {
                        m_Types[bodyID] = type;
                    }
                    else
                    {
                        Logger.Log("Warning: Invalid BodyType entry:");
                        Logger.Log(line);
                    }
                }
            }
        }
        else
        {
            Logger.Log("Warning: Data/BodyTable.cfg does not exist");

            m_Types = new BodyType[0];
        }
    }
}
