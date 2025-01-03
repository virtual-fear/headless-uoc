using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Game
{
    using Networking;
    using global::Client.Networking.Incoming;
    using static Client.Networking.Incoming.PacketSink;

    public interface IEntity
    {
        int Serial { get; }

        short X { get; }
        short Y { get; }
        sbyte Z { get; }
    }

    public interface IItemValidator
    {
        bool IsValid(Item check);
    }
    public interface IMobileValidator
    {
        bool IsValid(Mobile check);
    }

    public class World : WorldContent
    {
        public World() : base(0)
        {
        }

        public static void Configure()
        {
            // Configure all //
        }
    }

    public class WorldContent : Agent
    {
        private static string[] _worldNames;
        private static Dictionary<int, Item> _worldItems;
        private static Dictionary<int, Mobile> _worldMobiles;
        private static Int32 _worldRange = 12;

        private static Int32 m_Serial;
        private static Mobile m_Player;

        public static int Serial
        {
            get { return m_Serial; }
            set
            {
                m_Serial = value;
                m_Player = WorldContent.FindMobile(m_Serial);
            }
        }
        public static Mobile Player
        {
            get { return m_Player; }
        }
        public static bool Ingame
        {
            get { return (m_Player != null); }
        }
        public static Int32 Range
        {
            get { return _worldRange; }
        }

        static WorldContent()
        {
            _worldNames = new string[] { "Felucca", "Trammel", "Ilshenar", "Malas", "Tokunuo Islands" };

            _worldItems = new Dictionary<int, Item>();
            _worldMobiles = new Dictionary<int, Mobile>();
            
            _worldRange = 12;
        }

        
        public static void LoginConfirm(LoginConfirmEventArgs e)
        {
            m_Player = new Mobile(e.Serial);
            m_Player.SetLocation(e.X, e.Y, (sbyte)e.Z);
            //m_Player.Body = e.Body;
            //m_Player.Direction = e.Direction;
        }

        public WorldContent(int serial) : base(serial)
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

            return FindItem(delegate(Item item)
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
            return GetArray<Item>(GetItems(validator));
        }

        public static Mobile FindMobile(int serial)
        {
            Mobile m;
            _worldMobiles.TryGetValue(serial, out m);
            return m;
        }
        public static Mobile FindMobile(IMobileValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            return FindMobile(new Predicate<Mobile>(validator.IsValid));
        }
        public static Mobile FindMobile(params IMobileValidator[] validators)
        {
            if (validators == null)
                throw new ArgumentNullException("validators");

            return FindMobile(delegate(Mobile Mobile)
            {
                foreach (IMobileValidator validator in validators)
                {
                    if (validator.IsValid(Mobile))
                        return true;
                }
                return false;
            });
        }
        public static Mobile FindMobile(Predicate<Mobile> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            foreach (Mobile Mobile in _worldMobiles.Values)
            {
                if (validator.Invoke(Mobile))
                    return Mobile;
            }
            return null;
        }
        public static Mobile[] FindMobiles(IMobileValidator validator)
        {
            return GetArray<Mobile>(GetMobiles(validator));
        }

        public static Int32 GetAmount(params Item[] items)
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
        public static IEnumerable<Mobile> GetMobiles(IMobileValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            return GetMobiles(new Predicate<Mobile>(validator.IsValid));
        }
        public static IEnumerable<Mobile> GetMobiles(Predicate<Mobile> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");

            foreach (Mobile Mobile in _worldMobiles.Values)
            {
                yield return Mobile;
            }
        }
        
        public static void Remove(Item item)
        {
            if (item != null)
                item.Delete();
        }
        public static void Remove(Mobile mobile)
        {
            if (mobile != null)
                mobile.Delete();
        }

        public static Item WantItem(int serial)
        {
            Item item;

            if (!_worldItems.TryGetValue(serial, out item))
                _worldItems.Add(serial, (item = new Item(serial)));

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
        public static Mobile WantMobile(int serial)
        {
            Mobile mobile;

            if (!_worldMobiles.TryGetValue(serial, out mobile))
                _worldMobiles.Add(serial, (mobile = new Mobile(serial)));

            return mobile;
        }
        public static Mobile WantMobile(int serial, ref bool wasFound)
        {
            wasFound = false;

            Mobile mobile;

            if (_worldMobiles.TryGetValue(serial, out mobile))
            {
                wasFound = true;
                return mobile;
            }

            mobile = new Mobile(serial);
            _worldMobiles.Add(serial, mobile);

            return mobile;
        }
    }

}
