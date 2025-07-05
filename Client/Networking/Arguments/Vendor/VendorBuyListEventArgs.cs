namespace Client.Networking.Arguments;
using Client.Game;
public sealed class VendorBuyListEventArgs : EventArgs
{
    [PacketHandler(0x74, length: -1, ingame: true)]
    private static event PacketEventHandler<VendorBuyListEventArgs>? Update;
    public NetState State { get; }
    public int MenuSerial { get; }
    public int[] Prices { get; }
    public string[] Names { get; }
    private VendorBuyListEventArgs(NetState state, PacketReader ip)
    {
        // TODO: Fix localization
        State = state;
        MenuSerial = ip.ReadInt32();
        byte c = ip.ReadByte();
        int[] p = new int[c];
        string[] n = new string[c];
        if (c > 0)
        {
            for (int i = 0; i < c; ++i)
            {
                p[i] = ip.ReadInt32();
                n[i] = ip.ReadString(ip.ReadByte());
                //n[i] = Localization.GetString(Convert.ToInt32(n[i]));
            }
        }
        Prices = p;
        Names = n;
    }

    static VendorBuyListEventArgs() => Update += VendorBuyListEventArgs_Update;
    private static void VendorBuyListEventArgs_Update(VendorBuyListEventArgs e) => Vendor.OnBuy(e.MenuSerial, e.Names, e.Prices);
}