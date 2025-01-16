namespace Client.Networking.Incoming;
public partial class World
{
    public static event PacketEventHandler<ChangeUpdateRangeEventArgs>? OnChangeUpdateRange;
    public static event PacketEventHandler<CorpseEquipEventArgs>? OnUpdate_CorpseEquip;
    public static event PacketEventHandler<CreateWorldEntityEventArgs>? OnCreateEntity;
    public static event PacketEventHandler<CustomizedHouseContentEventArgs>? UpdateCustomizedHouseContent;
    public static event PacketEventHandler<GlobalLightEventArgs>? OnUpdate_GlobalLight;
    public static event PacketEventHandler<PersonalLightEventArgs>? OnUpdate_PersonalLight;
    public static event PacketEventHandler<PlayMusicEventArgs>? OnUpdate_PlayMusic;
    public static event PacketEventHandler<RemoveEventArgs>? OnRemove;
    public static event PacketEventHandler<SeasonChangeEventArgs>? OnUpdate_Season;
    public static event PacketEventHandler<PlaySoundEventArgs>? OnUpdate_Sound;
    public static event PacketEventHandler<WeatherEventArgs>? OnUpdate_Weather;
    public static event PacketEventHandler<WorldItemEventArgs>? OnCreateWorldItem;
    public static event PacketEventHandler<WorldItemIncomingEventArgs>? OnWorldItemIncoming;

    //[PacketHandler(????, length: -1, ingame: true)]
    internal static void Received_WorldIncomingItem(NetState ns, PacketReader ip) => OnWorldItemIncoming?.Invoke(new(ns, ip));

    [PacketHandler(0x1A, length: -1, ingame: true)]
    protected static void Received_CreateWorldItem(NetState ns, PacketReader ip) => OnCreateWorldItem?.Invoke(new(ns, ip));

    [PacketHandler(0x65, length: 4, ingame: true)]
    protected static void Received_Weather(NetState ns, PacketReader ip) => OnUpdate_Weather?.Invoke(new(ns, ip));
    
    [PacketHandler(0xBC, length: 3, ingame: true)]
    protected static void Received_Season(NetState ns, PacketReader ip) => OnUpdate_Season?.Invoke(new(ns, ip));

    [PacketHandler(0x1D, length: 5, ingame: true)]
    protected static void Received_RemoveObject(NetState ns, PacketReader ip) => OnRemove?.Invoke(new(ns, ip));

    [PacketHandler(0x54, length: 12, ingame: true)]
    protected static void Received_PlaySound(NetState ns, PacketReader ip) => OnUpdate_Sound?.Invoke(new(ns, ip));

    [PacketHandler(0x6D, length: 3, ingame: true)]
    protected static void Received_PlayMusic(NetState ns, PacketReader ip) => OnUpdate_PlayMusic?.Invoke(new(ns, ip));

    [PacketHandler(0x4E, length: 6, ingame: true)]
    protected static void Received_PersonalLight(NetState ns, PacketReader ip) => OnUpdate_PersonalLight?.Invoke(new(ns, ip));
    
    [PacketHandler(0x4F, length: 2, ingame: true)]
    protected static void Received_GlobalLight(NetState ns, PacketReader ip) => OnUpdate_GlobalLight?.Invoke(new(ns, ip));

    [PacketHandler(0xD8, length: -1, ingame: true)]
    protected static void Received_CustomizedHouseContent(NetState ns, PacketReader ip) => UpdateCustomizedHouseContent?.Invoke(new(ns, ip));
    //Item item = World.FindItem(serial);
    //if (((item != null) && (item.Multi != null)) && item.IsMulti)
    //{
    //    CustomMultiLoader.SetCustomMulti(serial, revision, item.Multi, compressionType, buffer);
    //}

    [PacketHandler(0xF3, length: 26, ingame: true)]
    protected static void Received_CreateWorldEntity(NetState ns, PacketReader pvSrc)
    {
        if (pvSrc.ReadInt16() != 0x1)
        {
            pvSrc.Trace();
            return;
        }
        OnCreateEntity?.Invoke(new(ns, pvSrc, isHS: false));
    }

    [PacketHandler(0x89, length: -1, ingame: true)]
    protected static void Received_CorpseEquip(NetState ns, PacketReader ip) => OnUpdate_CorpseEquip?.Invoke(new(ns, ip));

    [PacketHandler(0xC8, length: 2, ingame: true)]
    protected static void Received_ChangeUpdateRange(NetState ns, PacketReader ip) => OnChangeUpdateRange?.Invoke(new(ns, ip));
}