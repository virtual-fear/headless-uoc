namespace Client.Game
{
    using Client.Accounting;
    using Client.Game.Data;
    using global::Client.Networking.Incoming;
    using static global::Client.Networking.Incoming.PacketSink;
    public class Agent : IEntity
    {
        private Agent m_Parent;
        private int m_Serial;
        private short m_X, m_Y;
        private sbyte m_Z;

        private Dictionary<int, Agent> m_Children = new Dictionary<int, Agent>();

        public Agent Parent
        {
            get { return m_Parent; }
            private set
            {
                m_Parent = value;
                OnParentChanged();
            }
        }
        public int Serial
        {
            get { return m_Serial; }
        }
        public short X
        {
            get { return m_X; }
        }
        public short Y
        {
            get { return m_Y; }
        }
        public sbyte Z
        {
            get { return m_Z; }
        }
        //public bool Removed
        //{
        //    get { return m_Removed; }
        //}

        public IEnumerable<Agent> Children
        {
            get { return m_Children.Values; }
        }

        public Agent WorldRoot
        {
            get
            {
                for (Agent e = this; e.Parent != null; e = e.Parent)
                {
                    if (e is Agent && ((Agent)e).InWorld)
                        return e as Agent;
                }
                return null;
            }
        }

        public bool InWorld
        {
            get { return m_Parent is WorldContent; }
        }

        public Agent(int serial)
        {
            m_Serial = serial;
        }

        protected virtual void Attached(Agent child)
        {
        }

        protected virtual void Detached(Agent child)
        {
        }

        protected virtual void OnLocationChanged()
        {
        }

        protected virtual void OnParentChanged()
        {
        }

        protected virtual void OnDelete()
        {
        }

        private void Attach(Agent child)
        {
            if (child.Parent != this)
            {
                if (child.Parent != null)
                    child.Parent.Detach(child);

                if (child is Item ||
                    child is Mobile)
                {
                    m_Children[child.Serial] = child;
                }

                Attached(child);
                child.Parent = this;
            }
        }

        private void Detach(Agent child)
        {
            if (child.Parent == this)
            {
                if (m_Children.ContainsKey(child.Serial))
                    m_Children.Remove(child.Serial);

                Detach(child);
                child.Parent = null;
            }
        }

        public int DistanceTo(int x, int y)
        {
            x = m_X - x;
            y = m_Y - y;

            return (int)Math.Sqrt((x * x) + (y * y));
        }

        public bool IsChildOf(Agent agent)
        {
            if (agent != null)
            {
                for (Agent i = m_Parent; i != null; i = i.Parent)
                    if (i == agent)
                        return true;
            }
            return false;
        }

        public IAccount Account { get; set; } = null;
        public bool Deleted { get; set; } = false;

        public void Delete()
        {
            OnDelete();

            List<Agent> entities = new List<Agent>(m_Children.Values);

            while (entities.Count > 0)
                entities[entities.Count - 1].Delete();

            m_Children.Clear();
            Deleted = true;
        }

        public void SetLocation(short x, short y, sbyte z)
        {
            m_X = x;
            m_Y = y;
            m_Z = z;

            this.OnLocationChanged();
        }

        public void SetParent(Agent parent)
        {
            if (this.Parent != parent)
            {
                if (this.Parent != null)
                    this.Parent.Detach(this);

                if (parent != null)
                    parent.Attach(this);
            }
        }
    }

    public sealed class Mobile : Agent
    {
        public static Mobile Acquire(int serial)
        {
            return WorldContent.WantMobile(serial);
        }

        #region Fields
        private Direction _direction;
        private byte _sequence;
        private Notoriety _notoriety;
        private short _bodyID;
        private short _hue;
        private short _armor;
        private short _coldResistance;
        private short _energyResistance;
        private short _fireResistance;
        private short _poisonResistance;
        private short _maxWeaponDamage;
        private short _minWeaponDamage;
        private byte _followers;
        private byte _followersMax;
        private short _hits;
        private short _hitsMax;
        private short _mana;
        private short _manaMax;
        private short _stam;
        private short _stamMax;
        private short _weight;
        private short _weightMax;
        private short _str;
        private short _dex;
        private short _int;
        private short _statCapacity;
        private int _tithingPoints;
        private int _totalGold;
        private byte _raceID;
        private byte _gender;
        private bool _isPet;
        private short _luck;
        private string _name;
        private byte _typeID;
        private bool _warmode;
        #endregion

        public Mobile(int serial)
            : base(serial)
        {
        }

        protected override void OnLocationChanged()
        {
            base.OnLocationChanged();

            Logger.Log($"Mobile: {base.Serial}: moved to {base.X}, {base.Y}");
        }

        public static void Configure()
        {
            //PacketSink.LoginConfirm += WorldContent.LoginConfirm;
            PacketSink.MobileAnimation += OnAnimation;
            PacketSink.MobileAttributes += OnAttributes;
            PacketSink.MobileDamage += OnDamage;
            PacketSink.MobileHits += OnHits;
            PacketSink.MobileIncoming += OnIncoming;
            PacketSink.MobileMana += OnMana;
            PacketSink.MobileMoving += OnMoving;
            PacketSink.MobileStam += OnStam;
            PacketSink.MobileStatus += OnStatus;
            PacketSink.MobileUpdate += OnUpdate;
            PacketSink.MovementAck += OnMovementAck;
            PacketSink.MovementRej += OnMovementRej;
            PacketSink.SetWarMode += OnSetWarMode;
        }

        static void OnSetWarMode(SetWarModeEventArgs e)
        {
            WorldContent.Player.UpdateSetWarMode(e.Enabled);
        }

        private void UpdateSetWarMode(bool enabled)
        {
            _warmode = enabled;
        }

        static void OnMovementRej(MovementRejEventArgs e)
        {
            Mobile.Acquire(e.State.Mobile.Serial).UpdateMovementRej(e.Direction, e.Sequence, e.X, e.Y, e.Z);
        }

        private void UpdateMovementRej(Direction direction, byte sequence, short x, short y, sbyte z)
        {
            _direction = direction;
            _sequence = sequence;

            base.SetLocation(x, y, z);
        }

        static void OnMovementAck(MovementAckEventArgs e)
        {
            Mobile.Acquire(e.State.Mobile.Serial).UpdateMovementAck(e.Notoriety, e.Sequence);
        }

        private void UpdateMovementAck(Notoriety notoriety, byte sequence)
        {
            _notoriety = notoriety;
            _sequence = sequence;
        }

        static void OnUpdate(MobileUpdateEventArgs e)
        {
            Mobile.Acquire(e.Serial).Update((ushort)e.Body, e.Direction, e.Hue, e.X, e.Y, e.Z);
        }

        private void Update(ushort bodyID, Direction direction, short hue, short x, short y, sbyte z)
        {
            _bodyID = (short)bodyID;
            _direction = direction;
            _hue = hue;

            base.SetLocation(x, y, z);
        }

        static void OnStatus(MobileStatusEventArgs e)
        {
            Mobile.Acquire(e.Serial).UpdateStatus(e);
        }

        private void UpdateStatus(MobileStatusEventArgs e)
        {
            _armor = e.Armor;
            _coldResistance = e.ColdResistance;
            _dex = e.Dex;
            _energyResistance = e.EnergyResistance;
            _fireResistance = e.FireResistance;
            _followers = e.Followers;
            _gender = e.Gender;
            _hits = e.Hits;
            _int = e.Int;
            _isPet = e.IsPet;
            _luck = e.Luck;
            _mana = e.Mana;
            _followersMax = e.MaxFollowers;
            _hitsMax = e.MaxHits;
            _maxWeaponDamage = e.MaximumWeaponDamage;
            _manaMax = e.MaxMana;
            _stamMax = e.MaxStam;
            _weightMax = e.MaxWeight;
            _minWeaponDamage = e.MinimumWeaponDamage;
            _name = e.Name;
            _poisonResistance = e.PoisonResistance;
            _raceID = e.RaceID;
            _stam = e.Stam;
            _statCapacity = e.StatCap;
            _str = e.Str;
            _tithingPoints = e.TithingPoints;
            _totalGold = e.TotalGold;
            _typeID = e.Type;
            _weight = e.Weight;
        }

        static void OnStam(MobileStamEventArgs e)
        {
            Mobile.Acquire(e.Serial).UpdateStam(e.Stam, e.StamMax);
        }

        private void UpdateStam(short stam, short stamMax)
        {
            _stam = stam;
            _stamMax = stamMax;
        }

        static void OnMoving(MobileMovingEventArgs e)
        {
            Mobile.Acquire(e.Serial).UpdateMoving(e.Body, e.Direction, e.Hue, e.Notoriety, e.X, e.Y, e.Z);
        }

        private void UpdateMoving(short bodyID, Direction direction, short hue, Notoriety notoriety, short x, short y, sbyte z)
        {
            _bodyID = bodyID;
            _direction = direction;
            _hue = hue;
            _notoriety = notoriety;
            base.SetLocation(x, y, z);
        }

        static void OnMana(MobileManaEventArgs e)
        {
            Mobile.Acquire(e.Serial).UpdateMana(e.Mana, e.ManaMax);
        }

        private void UpdateMana(short mana, short manaMax)
        {
            _mana = mana;
            _manaMax = manaMax;
        }

        static void OnIncoming(MobileIncomingEventArgs e)
        {
            Mobile.Acquire(e.Serial).UpdateIncoming(e.Body, e.Direction, e.Hue, e.Notoriety, e.X, e.Y, e.Z);
        }

        private void UpdateIncoming(short bodyID, Direction direction, short hue, Notoriety notoriety, short x, short y, sbyte z)
        {
            _bodyID = bodyID;
            _direction = direction;
            _hue = hue;
            _notoriety = notoriety;
            base.SetLocation(x, y, z);
        }

        static void OnHits(MobileHitsEventArgs e)
        {
            Mobile.Acquire(e.Serial).UpdateHits(e.Hits, e.HitsMax);
        }

        private void UpdateHits(short hits, short hitsMax)
        {
            _hits = hits;
            _hitsMax = hitsMax;
        }

        static void OnDamage(MobileDamageEventArgs e)
        {
            Mobile m = Mobile.Acquire(e.Serial);

            short playerHealth = m._hits;
            playerHealth -= (short)e.Amount;

            if (playerHealth < 0)
                playerHealth = 0;

            Acquire(e.Serial).UpdateDamage(e.Amount);
        }

        private void UpdateDamage(ushort amount)
        {
            short health = _hits;
            health -= (short)amount;

            if (health < 0)
                health = 0;

            _hits = health;
        }

        static void OnAttributes(MobileAttributesEventArgs e)
        {
            Mobile.Acquire(e.Serial).UpdateAttributes(e.Hits, e.MaxHits, e.Mana, e.MaxMana, e.Stam, e.MaxStam);
        }

        private void UpdateAttributes(short hits, short hitsMax, short mana, short manaMax, short stam, short stamMax)
        {
            _hits = hits;
            _hitsMax = hitsMax;
            _mana = mana;
            _manaMax = manaMax;
            _stam = stam;
            _stamMax = stamMax;
        }

        static void OnAnimation(MobileAnimationEventArgs e)
        {
            Mobile.Acquire(e.Serial).UpdateAnimation((short)e.Action, e.Delay, e.Forward, (short)e.FrameCount, e.Repeat, (short)e.RepeatCount);
        }

        private void UpdateAnimation(short actionID, byte delay, bool forward, short frames, bool repeat, short count)
        {
        }
    }

    public sealed class Item : Agent
    {
        public int ID { get; private set; }
        public short Hue { get; private set; }
        public short Amount { get; private set; }
        public byte Flags { get; private set; }
        public Layer Layer { get; private set; }
        public static Item Acquire(int serial) => WorldContent.WantItem(serial);
        public Item(int serial) : base(serial) { }
        public static void Configure()
        {
            PacketSink.ItemIncoming += OnItemIncoming;
            PacketSink.Remove += OnRemoveItem;
            PacketSink.WorldItem += OnWorldItem;
        }
        private static void OnWorldItem(WorldItemEventArgs e)
        {
            var item = Item.Acquire(e.Serial);
            item.ID = e.ItemID;
            item.Hue = e.Hue;
            item.Amount = e.Amount;
            item.Flags = e.Flags;
            item.SetLocation(e.X, e.Y, e.Z);
        }

        private static void OnRemoveItem(RemoveEventArgs e)
        {
            var item = Item.Acquire(e.Serial);
            item.Delete();
        }
        static void OnItemIncoming(ItemIncomingEventArgs e)
        {
            var item = Item.Acquire(e.Serial);
            item.ID = e.ItemID;
            item.Hue = e.Hue;
            item.Layer = e.Layer;
        }

    }
}
