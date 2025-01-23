namespace Client.Game;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Client.Game.Data;
using Client.Networking;
using Client.Networking.Arguments;

public delegate void WorldEventHandler(World w);
public delegate void WorldEventChangedValue<T>(World w, T from, T? to);
public partial class World : Entity
{
    public const uint ItemOffset = 0x40000000;
    public const uint MaxItemSerial = 0x7EEEEEEE;

    public static event WorldEventChangedValue<Mobile>? ChangedCurrentPlayer;
    public static event WorldEventChangedValue<byte>? ChangedTileRange;
    public static event WorldEventChangedValue<int>? ChangedSpeed;

    #region Storage for items and mobiles

    private static Dictionary<Serial, Item> _worldItems = new();
    private static Dictionary<Serial, Mobile> _worldMobiles = new();

    public static Item WantItem(Serial serial)
    {
        Item item;

        if (!_worldItems.TryGetValue(serial, out item))
            _worldItems.Add(serial, item = new Item(serial));

        return item;
    }
    public static Item WantItem(Serial serial, ref bool wasFound)
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
    public static Mobile WantMobile(Serial serial)
    {
        Mobile mobile;

        if (!_worldMobiles.TryGetValue(serial, out mobile))
            _worldMobiles.Add(serial, mobile = new Mobile(serial));

        return mobile;
    }
    public static Mobile WantMobile(Serial serial, ref bool wasFound)
    {
        wasFound = false;
        if (_worldMobiles.TryGetValue(serial, out Mobile? mobile))
        {
            wasFound = true;
            return mobile;
        }
        mobile = new Mobile(serial);
        _worldMobiles.Add(serial, mobile);
        return mobile;
    }

    #endregion


    private static Mobile? _worldCurrentPlayer;
    public static Mobile? CurrentPlayer
    {
        get => _worldCurrentPlayer;
        set
        {
            var previousValue = _worldCurrentPlayer;
            _worldCurrentPlayer = value;
            if (previousValue != null)
                ChangedCurrentPlayer?.Invoke(World.Instance, previousValue, value);
        }
    }
    public static bool Ingame => CurrentPlayer != null;

    public static readonly World Instance = new();
    public World() : base((Serial)0) { }
    static World()
    {
        ChangedTileRange += World_ChangedTileRange;
    }

    private static void World_ChangedTileRange(World w, byte from, byte to)
        => Logger.Log($"The world tile range: {from} now set to {to}");
    internal static void Pause(NetState ns)
    {
        Logger.Log(ns.Address, $"The world has paused.");
    }
    internal static void CorpseEquip(CorpseEquipEventArgs e)
    {
        throw new NotImplementedException();
    }

    internal static void CreateEntity(CreateWorldEntityEventArgs e)
    {
        throw new NotImplementedException();
    }

    internal static void UpdateHouseContent(CustomizedHouseContentEventArgs e)
    {
        throw new NotImplementedException();
    }
    internal static void PlayMusic(NetState from, MusicName name)
    {
        Logger.Log(from.Address, $"Playing music: {name}");
    }

    internal static void PlaySound(PlaySoundEventArgs e)
    {
        throw new NotImplementedException();
    }

    internal static void Remove(Serial serial)
    {
        throw new NotImplementedException();
    }
    internal static void LoginComplete(NetState ns)
        => Logger.Log(ns.Address, "Logged in successfully!");

    private byte _worldTileRange = 18;
    private int _worldSpeedControl = 1;

    /// <summary>
    ///     The range of tiles we can view from our player.
    ///     <para><c>Outside assemblies will not be able to modify this property.</c></para>
    /// </summary>
    public byte TileRange
    {
        get => _worldTileRange;
        internal set // Outside assemblies will not be able to modify this property
        {
            var oldValue = _worldTileRange;
            if (oldValue != value)
            {
                _worldTileRange = value;
                ChangedTileRange?.Invoke(this, from: oldValue, to: value);
            }
        }
    }
    public int SpeedControl
    {
        get => _worldSpeedControl;
        internal set
        {
            var oldValue = _worldSpeedControl;
            if (oldValue != value)
            {
                _worldSpeedControl = value;
                Logger.Log($"The world speed has changed to {value}");
                ChangedSpeed?.Invoke(this, from: oldValue, to: value);
            }
        }
    }

    public static sbyte LightGlobalValue { get; internal set; }
    public static sbyte LightPersonalValue { get; internal set; }
    public static byte Season { get; internal set; }
    public static bool SeasonHasAudio { get; internal set; }
    public static WeatherEventArgs Weather { get; internal set; }
    
}