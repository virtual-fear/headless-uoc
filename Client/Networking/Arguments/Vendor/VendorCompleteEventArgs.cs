namespace Client.Networking.Arguments;
using Client.Game.Data;
public class VendorCompleteEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Vendor { get; }
    internal VendorCompleteEventArgs(NetState state, PacketReader pvSrc) {
        // { EndVendorSell, EndVendorBuy }
        State = state;
        pvSrc.ReadUInt16(); //  8   :   length
        Vendor = (Serial)pvSrc.ReadUInt32();
        pvSrc.ReadByte();   //  0x00

    }
}
