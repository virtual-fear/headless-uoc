namespace Client.Game.Context;
using Client.Game.Data;
using static Client.Networking.Incoming.Mobiles.PacketHandlers;
using static Client.Networking.Incoming.Movement.PacketHandlers;
public interface IMobileValidator
{
    bool IsValid(MobileContext check);
}
public class MobileContext : ContextEntity
{
    public static MobileContext Acquire(Serial serial) => WorldContext.WantMobile(serial);

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
    private string? _name;
    private byte _typeID;
    private bool _warmode;
    #endregion

    static MobileContext() => Configure();
    public MobileContext(Serial serial) : base(serial) { }
    protected override void OnChangedLocation()
    {
        base.OnChangedLocation();

        Logger.Log($"Mobile: {Serial}: moved to {X}, {Y}");
    }
    public static void Configure()
    {
        OnMobileAnimation += MobileContext_OnMobileAnimation;
        OnMobileAttributes += MobileContext_OnMobileAttributes;
        OnMobileDamage += MobileContext_OnMobileDamage;
        OnMobileHits += MobileContext_OnMobileHits;
        OnMobileIncoming += MobileContext_OnMobileIncoming;
        OnMobileMana += MobileContext_OnMobileMana;
        OnMobileMoving += MobileContext_OnMobileMoving;
        OnMobileStam += MobileContext_OnMobileStam;
        OnMobileStatus += MobileContext_OnMobileStatus;
        OnMobileUpdate += MobileContext_OnMobileUpdate;
        OnMovementAck += MobileContext_OnMovementAck;
        OnMovementRej += MobileContext_OnMovementRej;
        OnSetWarMode += MobileContext_OnSetWarMode;
    }

    private static void MobileContext_OnSetWarMode(SetWarModeEventArgs e)
    {
        WorldContext.Player.UpdateSetWarMode(e.Enabled);
    }
    private void UpdateSetWarMode(bool enabled) => _warmode = enabled;
    static void MobileContext_OnMovementRej(MovementRejEventArgs e)
    {
        if (e.State.Mobile is Mobile mob && mob != null)
        {
            Acquire(mob.Serial).UpdateMovementRej(e.Direction, e.Sequence, e.X, e.Y, e.Z);
        }
    }
    private void UpdateMovementRej(Direction direction, byte sequence, short x, short y, sbyte z)
    {
        _direction = direction;
        _sequence = sequence;

        SetLocation(x, y, z);
    }
    static void MobileContext_OnMovementAck(MovementAckEventArgs e)
    {
        if (e.State.Mobile == null)
            return;

        Acquire(e.State.Mobile.Serial).UpdateMovementAck(e.Notoriety, e.Sequence);
    }
    private void UpdateMovementAck(Notoriety notoriety, byte sequence)
    {
        _notoriety = notoriety;
        _sequence = sequence;
    }
    static void MobileContext_OnMobileUpdate(MobileUpdateEventArgs e)
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
    static void MobileContext_OnMobileStatus(MobileStatusEventArgs e)
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
    static void MobileContext_OnMobileStam(MobileStamEventArgs e)
    {
        Acquire(e.Serial).UpdateStam(e.Stam, e.StamMax);
    }
    private void UpdateStam(short stam, short stamMax)
    {
        _stam = stam;
        _stamMax = stamMax;
    }
    static void MobileContext_OnMobileMoving(MobileMovingEventArgs e)
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
    static void MobileContext_OnMobileMana(MobileManaEventArgs e)
    {
        Acquire(e.Serial).UpdateMana(e.Mana, e.ManaMax);
    }
    private void UpdateMana(short mana, short manaMax)
    {
        _mana = mana;
        _manaMax = manaMax;
    }
    static void MobileContext_OnMobileIncoming(MobileIncomingEventArgs e)
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
    static void MobileContext_OnMobileHits(MobileHitsEventArgs e)
    {
        Acquire(e.Serial).UpdateHits(e.Hits, e.HitsMax);
    }
    private void UpdateHits(short hits, short hitsMax)
    {
        _hits = hits;
        _hitsMax = hitsMax;
    }
    static void MobileContext_OnMobileDamage(MobileDamageEventArgs e)
    {
        MobileContext mob = Acquire(e.Serial);
        short playerHealth = mob._hits;
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
    static void MobileContext_OnMobileAttributes(MobileAttributesEventArgs e)
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
    static void MobileContext_OnMobileAnimation(MobileAnimationEventArgs e)
    {
        Acquire(e.Serial).UpdateAnimation((short)e.Action, e.Delay, e.Forward, (short)e.FrameCount, e.Repeat, (short)e.RepeatCount);
    }
    private void UpdateAnimation(short actionID, byte delay, bool forward, short frames, bool repeat, short count)
    {
    }
}

