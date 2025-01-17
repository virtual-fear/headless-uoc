namespace Client.Networking.Arguments;
public class VendorBuyListEventArgs : EventArgs
{
    public NetState State { get; }
    public int MenuSerial { get; }
    public int[] Prices { get; }
    public string[] Names { get; }
    internal VendorBuyListEventArgs(NetState state, PacketReader ip)
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
}