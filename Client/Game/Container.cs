namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public partial class Container
{
    [PacketHandler(0x3C, length: -1, ingame: true)]
    public static event PacketEventHandler<ContainerContentEventArgs>? OnContent;

    [PacketHandler(0x25, length: 20, ingame: true)]
    public static event PacketEventHandler<ContainerContentUpdateEventArgs>? OnContentUpdate;

    [PacketHandler(0x24, length: 7, ingame: true)]
    public static event PacketEventHandler<ContainerDisplayEventArgs>? OnDisplay;
}
