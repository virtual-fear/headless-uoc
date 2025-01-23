namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public class VendorSellListEventArgs : EventArgs
{
    [PacketHandler(0x9E, length: -1, ingame: true)]
    private static event PacketEventHandler<VendorSellListEventArgs>? Update;
    public NetState State { get; }
    public int ShopKeeper { get; set; }
    public VendorInfo[] Items { get; set; }
    private VendorSellListEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ShopKeeper = ip.ReadInt32();
        Items = new VendorInfo[ip.ReadUInt16()];
        for(int i = 0; i < Items.Length; i++)
            Items[i] = new VendorInfo(state, ip);
    }
    static VendorSellListEventArgs() => Update += VendorSellListEventArgs_Update;
    private static void VendorSellListEventArgs_Update(VendorSellListEventArgs e) => Vendor.OnSell(e.ShopKeeper, e.Items);
}
