namespace Client.Game.Context.Agents
{
    using static Client.Networking.Incoming.PacketSink;
    public class WorldAgent : Agent
    {
        private static string[] _worldNames;
        private static Dictionary<int, Item> _worldItems;
        private static Dictionary<int, MobileAgent> _worldMobiles;
        private static int _worldRange = 12;

        private static int m_Serial;
        private static MobileAgent m_Player;
        public static int Serial
        {
            get { return m_Serial; }
            set
            {
                m_Serial = value;
                m_Player = FindMobile(m_Serial);
            }
        }
        public static MobileAgent Player
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

        static WorldAgent()
        {
            _worldNames = new string[] { "Felucca", "Trammel", "Ilshenar", "Malas", "Tokunuo Islands" };
            _worldItems = new Dictionary<int, Item>();
            _worldMobiles = new Dictionary<int, MobileAgent>();
            _worldRange = 12;
        }


        public static void LoginConfirm(LoginConfirmEventArgs e)
        {
            m_Player = new MobileAgent(e.Serial);
            m_Player.SetLocation(e.X, e.Y, (sbyte)e.Z);
            //m_Player.Body = e.Body;
            //m_Player.Direction = e.Direction;
        }

        public WorldAgent(int serial) : base(serial)
        {
        }

        public static void Clear()
        {
            m_Serial = 0x00;
            m_Player = null;

            _worldMobiles.Clear();
            _worldItems.Clear();
        }

        public static Item FindItem(int serial)
        {
            Item item;
            _worldItems.TryGetValue(serial, out item);
            return item;
        }
        public static Item FindItem(IItemValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            return FindItem(new Predicate<Item>(validator.IsValid));
        }
        public static Item FindItem(params IItemValidator[] validators)
        {
            if (validators == null)
                throw new ArgumentNullException("validators");

            return FindItem(delegate (Item item)
            {
                foreach (IItemValidator validator in validators)
                {
                    if (validator.IsValid(item))
                        return true;
                }
                return false;
            });
        }
        public static Item FindItem(Predicate<Item> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            foreach (Item item in _worldItems.Values)
            {
                if (validator.Invoke(item))
                    return item;
            }
            return null;
        }
        public static Item[] FindItems(IItemValidator validator)
        {
            return GetArray(GetItems(validator));
        }

        public static MobileAgent FindMobile(int serial)
        {
            MobileAgent m;
            _worldMobiles.TryGetValue(serial, out m);
            return m;
        }
        public static MobileAgent FindMobile(IMobileValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            return FindMobile(new Predicate<MobileAgent>(validator.IsValid));
        }
        public static MobileAgent FindMobile(params IMobileValidator[] validators)
        {
            if (validators == null)
                throw new ArgumentNullException("validators");

            return FindMobile(delegate (MobileAgent Mobile)
            {
                foreach (IMobileValidator validator in validators)
                {
                    if (validator.IsValid(Mobile))
                        return true;
                }
                return false;
            });
        }
        public static MobileAgent FindMobile(Predicate<MobileAgent> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            foreach (MobileAgent Mobile in _worldMobiles.Values)
            {
                if (validator.Invoke(Mobile))
                    return Mobile;
            }
            return null;
        }
        public static MobileAgent[] FindMobiles(IMobileValidator validator)
        {
            return GetArray(GetMobiles(validator));
        }

        public static int GetAmount(params Item[] items)
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

        public static IEnumerable<Item> GetItems(IItemValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            return GetItems(new Predicate<Item>(validator.IsValid));
        }
        public static IEnumerable<Item> GetItems(Predicate<Item> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            foreach (Item item in _worldItems.Values)
            {
                yield return item;
            }
        }
        public static IEnumerable<MobileAgent> GetMobiles(IMobileValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            return GetMobiles(new Predicate<MobileAgent>(validator.IsValid));
        }
        public static IEnumerable<MobileAgent> GetMobiles(Predicate<MobileAgent> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            foreach (MobileAgent Mobile in _worldMobiles.Values)
            {
                yield return Mobile;
            }
        }

        public static void Remove(Item item)
        {
            if (item != null)
                item.Delete();
        }
        public static void Remove(MobileAgent mobile)
        {
            if (mobile != null)
                mobile.Delete();
        }

        public static Item WantItem(int serial)
        {
            Item item;

            if (!_worldItems.TryGetValue(serial, out item))
                _worldItems.Add(serial, item = new Item(serial));

            return item;
        }
        public static Item WantItem(int serial, ref bool wasFound)
        {
            wasFound = false;

            Item item;

            if (_worldItems.TryGetValue(serial, out item))
            {
                wasFound = true;
                return item;
            }

            item = new Item(serial);
            _worldItems.Add(serial, item);

            return item;
        }
        public static MobileAgent WantMobile(int serial)
        {
            MobileAgent mobile;

            if (!_worldMobiles.TryGetValue(serial, out mobile))
                _worldMobiles.Add(serial, mobile = new MobileAgent(serial));

            return mobile;
        }
        public static MobileAgent WantMobile(int serial, ref bool wasFound)
        {
            wasFound = false;

            MobileAgent mobile;

            if (_worldMobiles.TryGetValue(serial, out mobile))
            {
                wasFound = true;
                return mobile;
            }

            mobile = new MobileAgent(serial);
            _worldMobiles.Add(serial, mobile);

            return mobile;
        }
    }
}
