namespace Client.Game.Context.Data;
public struct Body
{
    public const string DataPath = @"data\bodytable.cfg";
    private static List<BodyType> BodyTypes = new List<BodyType>(capacity: 0x1000);
    public int BodyID { get; }
    public Body(int bodyID) => BodyID = bodyID;
    static Body()
    {
        if (File.Exists(DataPath))
        {
            using (StreamReader ip = new StreamReader(DataPath))
            {
                string? text;
                while ((text = ip.ReadLine()) != null)
                {
                    if (text.Length == 0 || text.StartsWith("#"))
                        continue;

                    string[] lines = text.Split('\t');
                    if (int.TryParse(lines[0], out int bodyID) &&
                        Enum.TryParse(lines[1], true, out BodyType bodyType) &&
                        bodyID >= 0 && bodyID < BodyTypes.Count)
                    {
                        BodyTypes[bodyID] = bodyType;
                    }
                    else
                    {
                        Logger.Log("Warning: Invalid BodyType entry:");
                        Logger.Log(text);
                    }
                }
            }
        }
        else
        {
            Logger.Log("Warning: Data/BodyTable.cfg does not exist");
        }
    }
    public BodyType Type
    {
        get
        {
            if (BodyID >= 0 && BodyID < BodyTypes.Count)
                return BodyTypes[BodyID];
            else
                return BodyType.Empty;
        }
    }
    public bool IsHumanoid
    {
        get
        {
            if (BodyID < 0 || BodyID >= BodyTypes.Count || BodyTypes[BodyID] != BodyType.Human)
                return false;

            return !IsGhost;
        }
    }
    public bool IsGargoyle => BodyID switch
    {
        666 or 667 or 694 or 695 => true,
        _ => false
    };
    public bool IsMale => BodyID switch
    {
        183 or 185 or 400 or 402 or 605 or 607 or 666 or 694 or 750 => true,
        _ => false
    };
    public bool IsFemale => BodyID switch
    {
        184 or 186 or 401 or 403 or 606 or 608 or 667 or 695 or 751 => true,
        _ => false
    };
    public bool IsGhost => BodyID switch
    {
        402 or 403 or 607 or 608 or 694 or 695 or 970 => true,
        _ => false
    };
    public bool IsMonster => CheckBodyType(BodyType.Monster);
    public bool IsAnimal => CheckBodyType(BodyType.Animal);
    public bool IsEmpty => CheckBodyType(BodyType.Empty);
    public bool IsSea => CheckBodyType(BodyType.Sea);
    private bool CheckBodyType(BodyType type) =>
        BodyID >= 0 && BodyID < BodyTypes.Count && BodyTypes[BodyID] == type;
    public bool IsEquipment
    {
        get { throw new NotImplementedException(); }
    }
    public override bool Equals(object obj) => obj != null && obj is Body b ? b.BodyID.Equals(BodyID) : false;
    public override int GetHashCode() => BodyID;
    public override string ToString() => string.Format("0x{0:X}", BodyID);

    public static implicit operator int(Body b) => b.BodyID;
    public static implicit operator Body(int bodyID) => new(bodyID);
    public static bool operator ==(Body l, Body r) => l.BodyID == r.BodyID;
    public static bool operator !=(Body l, Body r) => l.BodyID != r.BodyID;
    public static bool operator >(Body l, Body r) => l.BodyID > r.BodyID;
    public static bool operator >=(Body l, Body r) => l.BodyID >= r.BodyID;
    public static bool operator <(Body l, Body r) => l.BodyID < r.BodyID;
    public static bool operator <=(Body l, Body r) => l.BodyID <= r.BodyID;
}