namespace Client.Game;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Client.Game.Data;
using Client.Networking;
using Client.Networking.Arguments;

public delegate void WorldEventHandler(World w);
public delegate void WorldEventChangedValue<T>(World w, T from, T? to);
public sealed class WorldProperty<T> : ObservableProperty<World, T>
{
    internal WorldProperty(T? initialValue = default) : base(World.Instance, initialValue) { }

}
public partial class World : Entity
{
    public const uint ItemOffset = 0x40000000;
    public const uint MaxItemSerial = 0x7EEEEEEE;

    public static readonly World Instance = new();
    public static readonly WorldProperty<Mobile> Player = new();
    public static readonly WorldProperty<byte> TileRange = new(18);
    public static readonly WorldProperty<int> SpeedControl = new(1);
    public static readonly WorldProperty<sbyte> GlobalLight = new(1);
    public static readonly WorldProperty<sbyte> PersonalLight = new(1);
    public static readonly WorldProperty<byte> Season = new(0);
    public static readonly WorldProperty<bool> SeasonHasAudio = new(false);
    public static readonly WorldProperty<WeatherEventArgs?> Weather = new();

    #region Dictionary<Serial, Item/Mobile> _worldItems/_worldMobiles
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
    public static Mobile GetMobile(Serial serial)
    {
        Mobile mobile;
        if (_worldMobiles.ContainsKey(serial))
        {
            mobile = _worldMobiles[serial];
        } else
        {
            mobile = new Mobile(serial);
            _worldMobiles.Add(serial, mobile);
        }
        return mobile;
    }
    #endregion
    public static bool Ingame => (World.Player.Value != null);
    public World() : base((Serial)0) { }
    static World() => Configure();
    private static void Configure()
    {
        Player.ChangedValue += Player_ChangedValue;
        TileRange.ChangedValue += TileRange_ChangedValue;
        SpeedControl.ChangedValue += SpeedControl_ChangedValue;
        GlobalLight.ChangedValue += GlobalLight_ChangedValue;
        PersonalLight.ChangedValue += PersonalLight_ChangedValue;
        Season.ChangedValue += Season_ChangedValue;
        SeasonHasAudio.ChangedValue += SeasonHasAudio_ChangedValue;
        Weather.ChangedValue += Weather_ChangedValue;
    }
    private static void Weather_ChangedValue(World owner, WeatherEventArgs? from, WeatherEventArgs? to)
        => Logger.Log($"[World] Weather changed from ({from?.V1}, {from?.V2}, {from?.V3}) to ({to?.V1}, {to?.V2}, {to?.V3})");
    private static void SeasonHasAudio_ChangedValue(World owner, bool from, bool to)
        => Logger.Log($"[World] Season Audio changed from {from} to {to}");
    private static void Season_ChangedValue(World owner, byte from, byte to)
        => Logger.Log($"[World] Season changed from {from} to {to}");
    private static void PersonalLight_ChangedValue(World owner, sbyte from, sbyte to)
        => Logger.Log($"[World] Personal light changed from {from} to {to}");
    private static void GlobalLight_ChangedValue(World owner, sbyte from, sbyte to)
        => Logger.Log($"[World] Global light changed from {from} to {to}");
    private static void SpeedControl_ChangedValue(World owner, int from, int to)
        => Logger.Log($"[World] Speed control changed from {from} to {to}");
    private static void TileRange_ChangedValue(World owner, byte from, byte to)
        => Logger.Log($"[World] Tile range changed from {from} to {to}");
    private static void Player_ChangedValue(World owner, Mobile? from, Mobile? to)
        => Logger.Log($"[World] Player changed from {from?.Serial} to {to?.Serial}");
    internal static void Pause(NetState ns)
        => Logger.Log(ns.Address, $"[World] Paused.");
    internal static void CorpseEquip(CorpseEquipEventArgs e)
        => Logger.Log(e.State, $"[World] Corpse equip: {e.Beheld}");
    internal static void CreateEntity(CreateWorldEntityEventArgs e)
        => Logger.Log(e.State, $"[World] Creating entity itemID:{e.ItemID}, gfx:{e.GraphicsID}, type:{e.Type}, amt:{e.Amount}");
    internal static void UpdateHouseContent(CustomizedHouseContentEventArgs e)
        => Logger.Log(e.State, $"[World] House content updated: {e.Serial}");
    internal static void PlayAudio(NetState from, MusicName name)
        => Logger.Log(from.Address, $"Playing music: {name}");
    internal static void PlayAudio(PlaySoundEventArgs e)
        => Logger.Log(e.State.Address, $"Playing sound: {e.SoundID}");
    internal static void Remove(Serial serial)
        => Logger.Log($"[World] Removing serial: {serial}");
    internal static void LoginComplete(LoginCompleteEventArgs e)
        => Logger.Log(e.State.Address, $"{e.Mobile?.Name ?? "<MOBILE>"} logged in successfully!");
}