namespace Client.Networking.Incoming;
public partial class Container
{
    public static event PacketEventHandler<ContainerContentEventArgs>? OnContent;
    public static event PacketEventHandler<ContainerContentUpdateEventArgs>? OnContentUpdate;
    public static event PacketEventHandler<ContainerDisplayEventArgs>? OnDisplay;
    
    [PacketHandler(0x24, length: 7, ingame: true)]
    protected static void ReceivedContainer_Display(NetState ns, PacketReader ip) => OnDisplay?.Invoke(new(ns, ip));

    [PacketHandler(0x25, length: 20, ingame: true)]
    protected static void ReceivedContainer_ContentUpdate(NetState ns, PacketReader pvSrc) => OnContentUpdate?.Invoke(new(ns, pvSrc));

    [PacketHandler(0x3C, length: -1, ingame: true)]
    protected static void ReceivedContainer_Content(NetState ns, PacketReader ip) => OnContent?.Invoke(new(ns, ip));
}
