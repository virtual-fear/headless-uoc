namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public partial class Vendor
{
    [PacketHandler(0x74, length: -1, ingame: true)]
    public static event PacketEventHandler<VendorBuyListEventArgs>? OnBuyList;
        
    [PacketHandler(0x9E, length: -1, ingame: true)]
    public static event PacketEventHandler<VendorSellListEventArgs>? OnSellList;
        
    [PacketHandler(0x3B, length: 8, ingame: true)]
    public static event PacketEventHandler<VendorCompleteEventArgs>? OnComplete;
}
