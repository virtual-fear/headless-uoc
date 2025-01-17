namespace Client.Game;
using Client.Game.Context;
using Client.Game.Data;
using Client.Networking;
using Client.Networking.Arguments;
public partial class World : WorldContext
{
    public const uint ItemOffset = 0x40000000;
    public const uint MaxItemSerial = 0x7EEEEEEE;

    #region Networking Events
    [PacketHandler(0xC8, length: 2, ingame: true)]
    public static event PacketEventHandler<ChangeUpdateRangeEventArgs>? OnChangeUpdateRange;

    [PacketHandler(0x89, length: -1, ingame: true)]
    public static event PacketEventHandler<CorpseEquipEventArgs>? OnUpdate_CorpseEquip;

    [PacketHandler(0xF3, length: 26, ingame: true)]
    public static event PacketEventHandler<CreateWorldEntityEventArgs>? OnCreateEntity;

    [PacketHandler(0xD8, length: -1, ingame: true)]
    public static event PacketEventHandler<CustomizedHouseContentEventArgs>? UpdateCustomizedHouseContent;

    [PacketHandler(0x4F, length: 2, ingame: true)]
    public static event PacketEventHandler<GlobalLightEventArgs>? OnUpdate_GlobalLight;

    [PacketHandler(0x4E, length: 6, ingame: true)]
    public static event PacketEventHandler<PersonalLightEventArgs>? OnUpdate_PersonalLight;

    [PacketHandler(0x6D, length: 3, ingame: true)]
    public static event PacketEventHandler<PlayMusicEventArgs>? OnUpdate_PlayMusic;

    [PacketHandler(0x54, length: 12, ingame: true)]
    public static event PacketEventHandler<PlaySoundEventArgs>? OnUpdate_PlaySound;

    [PacketHandler(0x1D, length: 5, ingame: true)]
    public static event PacketEventHandler<RemoveEventArgs>? OnRemove;

    [PacketHandler(0xBC, length: 3, ingame: true)]
    public static event PacketEventHandler<SeasonChangeEventArgs>? OnUpdate_Season;

    [PacketHandler(0x65, length: 4, ingame: true)]
    public static event PacketEventHandler<WeatherEventArgs>? OnUpdate_Weather;

    [PacketHandler(0x1A, length: -1, ingame: true)]
    public static event PacketEventHandler<WorldItemEventArgs>? OnCreateWorldItem;
    public static event PacketEventHandler<WorldItemIncomingEventArgs>? OnWorldItemIncoming;

    //[PacketHandler(????, length: -1, ingame: true)]
    internal static void Received_WorldIncomingItem(NetState ns, PacketReader ip) => OnWorldItemIncoming?.Invoke(new(ns, ip));
    #endregion

    public World() : base((Serial)0) { }
    static World()
    {
        World.OnUpdate_GlobalLight += WorldEvent_OnUpdate_GlobalLight;
    }

    private static void WorldEvent_OnUpdate_GlobalLight(GlobalLightEventArgs e)
    {
        NetState ns = e.State;
        Logger.Log(ns.Address, "Updating global light");
    }


}