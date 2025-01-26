namespace Client.Game;

using System;
using Client.Game.Data;
using Client.Networking;
using Client.Networking.Arguments;
public interface IMobileValidator
{
    bool IsValid(Mobile check);
}
public class Mobile : Entity
{
    public static Mobile Acquire(Serial serial) => World.GetMobile(serial);

    #region Fields
    private Direction _direction;
    private byte _sequence;
    private Notoriety _notoriety;
    private short _bodyID;
    private short _hue;
    private byte _packetFlags;
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

    public bool IsDead { get; internal set; }
    public ushort Hits { get; internal set; }

    public short BodyID { get; internal set; }
    #endregion

    public Mobile(Serial serial) : base(serial) { }
    public string Name => _name ?? "*MOBILE*";
    protected override void OnChangedLocation(IPoint3D from, IPoint3D to)
    {
        base.OnChangedLocation(from, to);

        short x = to.X;
        short y = to.Y;

        Logger.Log($"Mobile: {Serial}: moved to {x}, {y}");
    }
    private void UpdateSetWarMode(bool enabled) => _warmode = enabled;
    private void UpdateIncoming(short bodyID, Direction direction, short hue, Notoriety notoriety, short x, short y, sbyte z)
    {
        _bodyID = bodyID;
        _direction = direction;
        _hue = hue;
        _notoriety = notoriety;
        Location = new Point3D() { X = x, Y = y, Z = z };
    }
    static void Mobile_OnChangedHits(MobileHitsEventArgs e)
    {
        Acquire(e.Serial).UpdateHits(e.Hits, e.HitsMax);
    }
    private void UpdateHits(short hits, short hitsMax)
    {
        _hits = hits;
        _hitsMax = hitsMax;
    }
    static void Mobile_ReceivedDamage(MobileDamageEventArgs e)
    {
        Mobile mob = Acquire(e.Serial);
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
    internal void Update(MobileAnimationEventArgs e)
        => UpdateAnimation(e.State, (short)e.Action, e.Delay, e.Forward, (short)e.FrameCount, e.Repeat, (short)e.RepeatCount);
    internal void UpdateAnimation(NetState ns, short actionID, byte delay, bool forward, short frames, bool repeat, short count)
    {
        // TODO: Mobile event call for the mobile and not the network args
    }
    internal void Update(DamageEventArgs e) => OnDamage(e.Amount);
    private void OnDamage(ushort amount)
    {
        if (amount == 0)
            throw new InvalidDataException(message: "Amount should be greater than zero");

        _hits -= (short)amount;

        if (_hits < 0)
            _hits = 0;

        OnHealthChange();
    }
    private void OnHealthChange() => Logger.Log($"({this.Serial}) {_name} health changed to {_hits}");
    internal void Update(LoginConfirmEventArgs e)
    {
        Player.Mobile = e.State.Mobile = this;
        _bodyID = e.BodyID;
        _direction = e.Direction;
        // TODO: Check width/height?
        Location = e.Location;
    }
    internal void UpdateAttributes(short hits, short maxHits, short mana, short maxMana, short stam, short maxStam)
    {
        _hits = hits;
        _hitsMax = maxHits;
        _mana = mana;
        _manaMax = maxMana;
        _stam = stam;
        _stamMax = maxStam;
    }
    internal void UpdateStatus(MobileStatusEventArgs e)
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
    internal void UpdateHealth(short hits, short hitsMax)
    {
        throw new NotImplementedException();
    }
    internal void UpdateMana(short mana, short manaMax)
    {
        throw new NotImplementedException();
    }
    internal void UpdateIncoming(MobileIncomingEventArgs e)
    {
        throw new NotImplementedException();
    }
    internal void UpdateStamina(NetState ns, short stamina, short maxValue)
    {
        Logger.Log(ns.Address, $"Updating stamina: {stamina}, max-value: {maxValue}");
        _stam = stamina;
        _stamMax = maxValue;
    }
    internal void Update(NetState ns, short bodyID, short hue, byte packetFlags, Direction dir, IPoint3D loc, Notoriety? noto = null)
    {
        Logger.Log(ns.Address, $"Updating mobile body: {bodyID}, hue: {hue}, flags: {packetFlags}, direction: {dir}, location: {loc}");
        _bodyID = bodyID;
        _hue = hue;
        _packetFlags = packetFlags;
        _direction = dir;
        if ((noto != null) && noto is Notoriety n)
            _notoriety = n;
        Location = loc;
    }
    internal static void OnMovementAck(NetState ns, byte sequence, Notoriety notoriety)
    {
        throw new NotImplementedException();
    }
    internal static void OnMovementRej(NetState ns, byte sequence, Direction direction, IPoint3D location)
    {
        throw new NotImplementedException();
    }
    internal void UpdateAnimation(NetState ns, byte status, byte animation, byte frame)
    {
        throw new NotImplementedException();
    }
    internal void OnMove(NetState ns, short bodyID, short hue, byte packetFlags, Notoriety noto, Direction dir, IPoint3D loc)
    {
        Logger.Log(ns.Address, $"Mobile: {Serial} moved {dir} ({loc.X}, {loc.Y})");
        _bodyID = bodyID;
        _hue = hue;
        _packetFlags = packetFlags;
        _notoriety = noto;
        _direction = dir;
        Location = loc;
    }
}