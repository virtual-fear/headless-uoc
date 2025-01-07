namespace Client.Game.Context;

using Client;
using Client.Game.Context.Data;
using static Client.Networking.Incoming.PacketSink;
public interface IMobileValidator
{
    bool IsValid(MobileAgent check);
}
public sealed class MobileAgent : Agent
{
    public static MobileAgent Acquire(int serial)
    {
        return WorldAgent.WantMobile(serial);
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

    public MobileAgent(int serial)
        : base(serial)
    {
    }

    protected override void OnChangedLocation()
    {
        base.OnChangedLocation();

        Logger.Log($"Mobile: {Serial}: moved to {X}, {Y}");
    }

    public static void Configure()
    {
        //PacketSink.LoginConfirm += WorldContent.LoginConfirm;
        MobileAnimation += OnAnimation;
        MobileAttributes += OnAttributes;
        MobileDamage += OnDamage;
        MobileHits += OnHits;
        MobileIncoming += OnIncoming;
        MobileMana += OnMana;
        MobileMoving += OnMoving;
        MobileStam += OnStam;
        MobileStatus += OnStatus;
        MobileUpdate += OnUpdate;
        MovementAck += OnMovementAck;
        MovementRej += OnMovementRej;
        SetWarMode += OnSetWarMode;
    }

    static void OnSetWarMode(SetWarModeEventArgs e)
    {
        WorldAgent.Player.UpdateSetWarMode(e.Enabled);
    }

    private void UpdateSetWarMode(bool enabled)
    {
        _warmode = enabled;
    }

    static void OnMovementRej(MovementRejEventArgs e)
    {
        MobileAgent.Acquire(e.State.Mobile.Serial).UpdateMovementRej(e.Direction, e.Sequence, e.X, e.Y, e.Z);
    }

    private void UpdateMovementRej(Direction direction, byte sequence, short x, short y, sbyte z)
    {
        _direction = direction;
        _sequence = sequence;

        SetLocation(x, y, z);
    }

    static void OnMovementAck(MovementAckEventArgs e)
    {
        MobileAgent.Acquire(e.State.Mobile.Serial).UpdateMovementAck(e.Notoriety, e.Sequence);
    }

    private void UpdateMovementAck(Notoriety notoriety, byte sequence)
    {
        _notoriety = notoriety;
        _sequence = sequence;
    }

    static void OnUpdate(MobileUpdateEventArgs e)
    {
        Acquire(e.Serial).Update((ushort)e.Body, e.Direction, e.Hue, e.X, e.Y, e.Z);
    }

    private void Update(ushort bodyID, Direction direction, short hue, short x, short y, sbyte z)
    {
        _bodyID = (short)bodyID;
        _direction = direction;
        _hue = hue;

        SetLocation(x, y, z);
    }

    static void OnStatus(MobileStatusEventArgs e)
    {
        Acquire(e.Serial).UpdateStatus(e);
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
        Acquire(e.Serial).UpdateStam(e.Stam, e.StamMax);
    }

    private void UpdateStam(short stam, short stamMax)
    {
        _stam = stam;
        _stamMax = stamMax;
    }

    static void OnMoving(MobileMovingEventArgs e)
    {
        Acquire(e.Serial).UpdateMoving(e.Body, e.Direction, e.Hue, e.Notoriety, e.X, e.Y, e.Z);
    }

    private void UpdateMoving(short bodyID, Direction direction, short hue, Notoriety notoriety, short x, short y, sbyte z)
    {
        _bodyID = bodyID;
        _direction = direction;
        _hue = hue;
        _notoriety = notoriety;
        SetLocation(x, y, z);
    }

    static void OnMana(MobileManaEventArgs e)
    {
        Acquire(e.Serial).UpdateMana(e.Mana, e.ManaMax);
    }

    private void UpdateMana(short mana, short manaMax)
    {
        _mana = mana;
        _manaMax = manaMax;
    }

    static void OnIncoming(MobileIncomingEventArgs e)
    {
        Acquire(e.Serial).UpdateIncoming(e.Body, e.Direction, e.Hue, e.Notoriety, e.X, e.Y, e.Z);
    }

    private void UpdateIncoming(short bodyID, Direction direction, short hue, Notoriety notoriety, short x, short y, sbyte z)
    {
        _bodyID = bodyID;
        _direction = direction;
        _hue = hue;
        _notoriety = notoriety;
        SetLocation(x, y, z);
    }

    static void OnHits(MobileHitsEventArgs e)
    {
        Acquire(e.Serial).UpdateHits(e.Hits, e.HitsMax);
    }

    private void UpdateHits(short hits, short hitsMax)
    {
        _hits = hits;
        _hitsMax = hitsMax;
    }

    static void OnDamage(MobileDamageEventArgs e)
    {
        MobileAgent m = Acquire(e.Serial);

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
        Acquire(e.Serial).UpdateAttributes(e.Hits, e.MaxHits, e.Mana, e.MaxMana, e.Stam, e.MaxStam);
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
        Acquire(e.Serial).UpdateAnimation((short)e.Action, e.Delay, e.Forward, (short)e.FrameCount, e.Repeat, (short)e.RepeatCount);
    }

    private void UpdateAnimation(short actionID, byte delay, bool forward, short frames, bool repeat, short count)
    {
    }
}

public interface IItemValidator
{
    bool IsValid(Item check);
}
public sealed class Item : Agent
{
    public int ID { get; private set; }
    public short Hue { get; private set; }
    public short Amount { get; private set; }
    public byte Flags { get; private set; }
    public Layer Layer { get; private set; }
    public static Item Acquire(int serial) => WorldAgent.WantItem(serial);
    public Item(int serial) : base(serial) { }
    public static void Configure()
    {
        ItemIncoming += OnItemIncoming;
        Remove += OnRemoveItem;
        WorldItem += OnWorldItem;
    }
    private static void OnWorldItem(WorldItemEventArgs e)
    {
        var item = Acquire(e.Serial);
        item.ID = e.ItemID;
        item.Hue = e.Hue;
        item.Amount = e.Amount;
        item.Flags = e.Flags;
        item.SetLocation(e.X, e.Y, e.Z);
    }
    private static void OnRemoveItem(RemoveEventArgs e)
    {
        var item = Acquire(e.Serial);
        item.Delete();
    }
    static void OnItemIncoming(ItemIncomingEventArgs e)
    {
        var item = Acquire(e.Serial);
        item.ID = e.ItemID;
        item.Hue = e.Hue;
        item.Layer = e.Layer;
    }
}