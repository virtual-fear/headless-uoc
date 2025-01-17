namespace Client.Networking.Arguments;
using Client.Game.Data;
public class VendorSellListEventArgs : EventArgs
{
    public NetState State { get; }
    public int ShopKeeper { get; set; }
    public VendorInfo[] Items { get; set; }
    internal VendorSellListEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ShopKeeper = ip.ReadInt32();
        Items = new VendorInfo[ip.ReadUInt16()];
        for(int i = 0; i < Items.Length; i++)
            Items[i] = new VendorInfo(state, ip);
    }
}
