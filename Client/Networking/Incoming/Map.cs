namespace Client.Networking.Incoming;
public partial class Map
{
    public static event PacketEventHandler<MapChangeEventArgs>? OnChange;
    public static event PacketEventHandler<MapCommandEventArgs>? OnCommand;
    //[Obsolete] public static event PacketEventHandler<MapDetailsEventArgs>? OnDetails;
    public static event PacketEventHandler<MapPatchesEventArgs>? OnPatch;

    [PacketHandler(0x18, length: 33, ingame: true, extCmd: true)]
    protected static void ReceivedMap_Patches(NetState ns, PacketReader ip) => OnPatch?.Invoke(new(ns, ip));

    //[Obsolete] private static void ReceivedMap_Details(NetState ns, PacketReader pvSrc) => OnDetails?.Invoke(new(ns, pvSrc));
    
    [PacketHandler(0x56, length: 11, ingame: true)]
    protected static void ReceivedMap_Command(NetState ns, PacketReader pvSrc) => OnCommand?.Invoke(new(ns, pvSrc));

    [PacketHandler(0x8, length: 6, ingame: true, extCmd: true)]
    protected static void ReceivedMap_Change(NetState ns, PacketReader ip) => OnChange?.Invoke(new(ns, ip));
}
