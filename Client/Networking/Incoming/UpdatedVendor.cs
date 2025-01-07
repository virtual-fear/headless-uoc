namespace Client.Networking.Incoming;

using static PacketSink;
public partial class PacketSink
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
        public SellInfo[] Items { get; set; }
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

    public static event PacketEventHandler<VendorBuyListEventArgs>? VendorBuyList;
    public static event PacketEventHandler<VendorSellListEventArgs>? VendorSellList;
    public static event PacketEventHandler<VendorCompleteEventArgs>? VendorComplete;
    public static void InvokeVendorComplete(VendorCompleteEventArgs e) => VendorComplete?.Invoke(e);
    public static void InvokeVendorSellList(VendorSellListEventArgs e) => VendorSellList?.Invoke(e);
    public static void InvokeVendorBuyList(VendorBuyListEventArgs e) => VendorBuyList?.Invoke(e);
}
public sealed class SellInfo
{
    public NetState State { get; }
    public SellInfo(NetState state) => State = state;
    public int Item { get; set; }
    public ushort ID { get; set; }
    public ushort Hue { get; set; }
    public ushort Amount { get; set; }
    public ushort Price { get; set; }
    public string Name { get; set; }
}

public static class UpdatedVendor
{
    public static void Configure()
    {
        Register(0x74, -1, true, new OnPacketReceive(VendorBuyList));   // TODO: fix localization
        Register(0x9E, -1, true, new OnPacketReceive(VendorSellList));
        Register(0x3B, 08, true, new OnPacketReceive(VendorComplete));  //  { EndVendorSell, EndVendorBuy }
    }

    private static void VendorComplete(NetState ns, PacketReader pvSrc)
    {
        VendorCompleteEventArgs e = new VendorCompleteEventArgs(ns);

        pvSrc.ReadUInt16(); //  8   :   length
        e.Vendor = pvSrc.ReadInt32();
        pvSrc.ReadByte();   //  0x00

        PacketSink.InvokeVendorComplete(e);
    }

    private static void VendorSellList(NetState ns, PacketReader pvSrc)
    {
        VendorSellListEventArgs e = new VendorSellListEventArgs(ns);

        e.ShopKeeper = pvSrc.ReadInt32();

        List<SellInfo> list = new List<SellInfo>(pvSrc.ReadUInt16());
        for (int i = 0; i < list.Capacity; ++i)
        {
            SellInfo si = new SellInfo(ns);

            si.Item = pvSrc.ReadInt32();
            si.ID = pvSrc.ReadUInt16();
            si.Hue = pvSrc.ReadUInt16();
            si.Amount = pvSrc.ReadUInt16();
            si.Price = pvSrc.ReadUInt16();
            si.Name = pvSrc.ReadString(pvSrc.ReadUInt16());

            list.Add(si);
        }

        e.Items = list.ToArray();

        PacketSink.InvokeVendorSellList(e);
    }

    private static void VendorBuyList(NetState ns, PacketReader pvSrc)
    {
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
        PacketSink.InvokeVendorBuyList(e);
    }

    private static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive) => PacketHandlers.Register(packetID, length, variable, onReceive);
}
