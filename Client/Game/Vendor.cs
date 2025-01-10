namespace Client.Game;
using Client.Networking;
public partial class PacketHandlers
{
    #region EventArgs
    public class VendorCompleteEventArgs : EventArgs
    {
        public NetState State { get; }
        public VendorCompleteEventArgs(NetState state) => State = state;
        public int Vendor { get; set; }
    }
    public class VendorSellListEventArgs : EventArgs
    {
        public NetState State { get; }
        public VendorSellListEventArgs(NetState state) => State = state;
        public int ShopKeeper { get; set; }
        public VendorInfo[] Items { get; set; }
    }
    public class VendorBuyListEventArgs : EventArgs
    {
        public NetState State { get; }
        public VendorBuyListEventArgs(NetState state) => State = state;
        public int MenuSerial { get; set; }
        public int[] Prices { get; set; }
        public string[] Names { get; set; }
    }

    #endregion (done)

    public static event PacketEventHandler<VendorBuyListEventArgs>? Vendor_BuyList;
    public static event PacketEventHandler<VendorSellListEventArgs>? Vendor_SellList;
    public static event PacketEventHandler<VendorCompleteEventArgs>? Vendor_Complete;
    public sealed class VendorInfo
    {
        public NetState State { get; }
        public VendorInfo(NetState state) => State = state;
        public int Item { get; set; }
        public ushort ID { get; set; }
        public ushort Hue { get; set; }
        public ushort Amount { get; set; }
        public ushort Price { get; set; }
        public string Name { get; set; }
    }
    protected static class Vendor
    {
        [PacketHandler(0x3B, length: 8, ingame: true)]
        internal static void Complete(NetState ns, PacketReader pvSrc)
        {
            // { EndVendorSell, EndVendorBuy }
            VendorCompleteEventArgs e = new(ns);
            pvSrc.ReadUInt16(); //  8   :   length
            e.Vendor = pvSrc.ReadInt32();
            pvSrc.ReadByte();   //  0x00
            Vendor_Complete?.Invoke(e);
        }

        [PacketHandler(0x9E, length: -1, ingame: true)]
        internal static void SellList(NetState ns, PacketReader pvSrc)
        {
            VendorSellListEventArgs e = new(ns);
            e.ShopKeeper = pvSrc.ReadInt32();
            List<VendorInfo> list = new List<VendorInfo>(pvSrc.ReadUInt16());
            for (int i = 0; i < list.Capacity; ++i)
            {
                VendorInfo si = new VendorInfo(ns);

                si.Item = pvSrc.ReadInt32();
                si.ID = pvSrc.ReadUInt16();
                si.Hue = pvSrc.ReadUInt16();
                si.Amount = pvSrc.ReadUInt16();
                si.Price = pvSrc.ReadUInt16();
                si.Name = pvSrc.ReadString(pvSrc.ReadUInt16());

                list.Add(si);
            }
            e.Items = list.ToArray();
            Vendor_SellList?.Invoke(e);
        }

        [PacketHandler(0x74, length: -1, ingame: true)]
        internal static void BuyList(NetState ns, PacketReader pvSrc)
        {
            // TODO: Fix localization
            VendorBuyListEventArgs e = new VendorBuyListEventArgs(ns);
            e.MenuSerial = pvSrc.ReadInt32();
            byte c = pvSrc.ReadByte();
            int[] p = new int[c];
            string[] n = new string[c];
            if (c > 0)
            {
                for (int i = 0; i < c; ++i)
                {
                    p[i] = pvSrc.ReadInt32();
                    n[i] = pvSrc.ReadString(pvSrc.ReadByte());
                    //n[i] = Localization.GetString(Convert.ToInt32(n[i]));
                }
                // todo: fix localization
            }
            e.Prices = p;
            e.Names = n;
            Vendor_BuyList?.Invoke(e);
        }
    }
}