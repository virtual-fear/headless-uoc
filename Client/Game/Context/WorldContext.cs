namespace Client.Game.Context;
using static Client.Networking.Incoming.Shard.PacketHandlers;
public class WorldContext : ContextEntity
{
    private static string[] _worldNames;
    private static Dictionary<int, ItemContext> _worldItems;
    private static Dictionary<int, MobileContext> _worldMobiles;
    private static int _worldRange = 12;

    private static int m_Serial;
    private static MobileContext m_Player;
    public static int Serial
    {
        get { return m_Serial; }
        set
        {
            m_Serial = value;
            m_Player = FindMobile(m_Serial);
        }
    }
    public static MobileContext Player
    {
        get { return m_Player; }
    }
    public static bool Ingame
    {
        get { return m_Player != null; }
    }
    public static int Range
    {
        get { return _worldRange; }
    }

    static WorldContext()
    {
        _worldNames = new string[] { "Felucca", "Trammel", "Ilshenar", "Malas", "Tokunuo Islands" };
        _worldItems = new Dictionary<int, ItemContext>();
        _worldMobiles = new Dictionary<int, MobileContext>();
        _worldRange = 12;
    }


    public static void LoginConfirm(LoginConfirmEventArgs e)
    {
        m_Player = new MobileContext(e.Serial);
        m_Player.SetLocation(e.X, e.Y, (sbyte)e.Z);
        //m_Player.Body = e.Body;
        //m_Player.Direction = e.Direction;
    }

    public WorldContext(int serial) : base(serial)
    {
    }

    public static void Clear()
    {
        m_Serial = 0x00;
        m_Player = null;

        _worldMobiles.Clear();
        _worldItems.Clear();
    }

    public static ItemContext FindItem(int serial)
    {
        ItemContext item;
        _worldItems.TryGetValue(serial, out item);
        return item;
    }
    public static ItemContext FindItem(IItemValidator validator)
    {
        if (validator == null)
            throw new ArgumentNullException("validator");

        return FindItem(new Predicate<ItemContext>(validator.IsValid));
    }
    public static ItemContext FindItem(params IItemValidator[] validators)
    {
        if (validators == null)
            throw new ArgumentNullException("validators");

        return FindItem(delegate (ItemContext item)
        {
            foreach (IItemValidator validator in validators)
            {
                if (validator.IsValid(item))
                    return true;
            }
            return false;
        });
    }
    public static ItemContext FindItem(Predicate<ItemContext> validator)
    {
        if (validator == null)
            throw new ArgumentNullException("validator");

        foreach (ItemContext item in _worldItems.Values)
        {
            if (validator.Invoke(item))
                return item;
        }
        return null;
    }
    public static ItemContext[] FindItems(IItemValidator validator)
    {
        return GetArray(GetItems(validator));
    }

    public static MobileContext FindMobile(int serial)
    {
        MobileContext m;
        _worldMobiles.TryGetValue(serial, out m);
        return m;
    }
    public static MobileContext FindMobile(IMobileValidator validator)
    {
        if (validator == null)
            throw new ArgumentNullException("validator");

        return FindMobile(new Predicate<MobileContext>(validator.IsValid));
    }
    public static MobileContext FindMobile(params IMobileValidator[] validators)
    {
        if (validators == null)
            throw new ArgumentNullException("validators");

        return FindMobile(delegate (MobileContext Mobile)
        {
            foreach (IMobileValidator validator in validators)
            {
                if (validator.IsValid(Mobile))
                    return true;
            }
            return false;
        });
    }
    public static MobileContext FindMobile(Predicate<MobileContext> validator)
    {
        if (validator == null)
            throw new ArgumentNullException("validator");

        foreach (MobileContext Mobile in _worldMobiles.Values)
        {
            if (validator.Invoke(Mobile))
                return Mobile;
        }
        return null;
    }
    public static MobileContext[] FindMobiles(IMobileValidator validator)
    {
        return GetArray(GetMobiles(validator));
    }

    public static int GetAmount(params ItemContext[] items)
    {
        int n = 0x00;

        //for (int i = 0; i < items.Length; ++i)
        //    n += items[i].Amount;

        return n;
    }

    private static T[] GetArray<T>(IEnumerable<T> collection)
    {
        List<T> list = new List<T>();
        list.AddRange(collection);
        T[] array = list.ToArray();
        list.Clear();
        return array;
    }

    public static string GetIndex(int i)
    {
        if (i > _worldNames.Length)
            i = -1;

        return _worldNames[i];
    }

    public static IEnumerable<ItemContext> GetItems(IItemValidator validator)
    {
        if (validator == null)
            throw new ArgumentNullException("validator");

        return GetItems(new Predicate<ItemContext>(validator.IsValid));
    }
    public static IEnumerable<ItemContext> GetItems(Predicate<ItemContext> validator)
    {
        if (validator == null)
            throw new ArgumentNullException("validator");

        foreach (ItemContext item in _worldItems.Values)
        {
            yield return item;
        }
    }
    public static IEnumerable<MobileContext> GetMobiles(IMobileValidator validator)
    {
        if (validator == null)
            throw new ArgumentNullException("validator");

        return GetMobiles(new Predicate<MobileContext>(validator.IsValid));
    }
    public static IEnumerable<MobileContext> GetMobiles(Predicate<MobileContext> validator)
    {
        if (validator == null)
            throw new ArgumentNullException("validator");

        foreach (MobileContext Mobile in _worldMobiles.Values)
        {
            yield return Mobile;
        }
    }

    public static void Remove(ItemContext item)
    {
        if (item != null)
            item.Delete();
    }
    public static void Remove(MobileContext mobile)
    {
        if (mobile != null)
            mobile.Delete();
    }

    public static ItemContext WantItem(int serial)
    {
        ItemContext item;

        if (!_worldItems.TryGetValue(serial, out item))
            _worldItems.Add(serial, item = new ItemContext(serial));

        return item;
    }
    public static ItemContext WantItem(int serial, ref bool wasFound)
    {
        wasFound = false;

        ItemContext item;

        if (_worldItems.TryGetValue(serial, out item))
        {
            wasFound = true;
            return item;
        }

        item = new ItemContext(serial);
        _worldItems.Add(serial, item);

        return item;
    }
    public static MobileContext WantMobile(int serial)
    {
        MobileContext mobile;

        if (!_worldMobiles.TryGetValue(serial, out mobile))
            _worldMobiles.Add(serial, mobile = new MobileContext(serial));

        return mobile;
    }
    public static MobileContext WantMobile(int serial, ref bool wasFound)
    {
        wasFound = false;

        MobileContext mobile;

        if (_worldMobiles.TryGetValue(serial, out mobile))
        {
            wasFound = true;
            return mobile;
        }

        mobile = new MobileContext(serial);
        _worldMobiles.Add(serial, mobile);

        return mobile;
    }
}
