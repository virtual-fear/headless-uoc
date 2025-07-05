namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class VendorCompleteEventArgs : EventArgs
{
    [PacketHandler(0x3B, length: 8, ingame: true)]
    public static event PacketEventHandler<VendorCompleteEventArgs>? Update;

    public NetState State { get; }
    public Serial VendorSerial { get; }
    private VendorCompleteEventArgs(NetState state, PacketReader pvSrc)
    {
        // { EndVendorSell, EndVendorBuy }
        State = state;
        pvSrc.ReadUInt16(); //  8   :   length
        VendorSerial = (Serial)pvSrc.ReadUInt32();
        pvSrc.ReadByte();   //  0x00
    }

    static VendorCompleteEventArgs() => Update += VendorCompleteEventArgs_Update;
    private static void VendorCompleteEventArgs_Update(VendorCompleteEventArgs e) => Vendor.OnComplete(e.VendorSerial);
}