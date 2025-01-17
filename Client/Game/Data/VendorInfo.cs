namespace Client.Game.Data;
using Client.Networking;
public sealed class VendorInfo
{
    public NetState State { get; }
    public int Item { get; }
    public ushort ID { get; }
    public ushort Hue { get; }
    public ushort Amount { get; }
    public ushort Price { get; }
    public string Name { get; }
    internal VendorInfo(NetState state, PacketReader pvSrc)
    {
        State = state;
        Item = pvSrc.ReadInt32();
        ID = pvSrc.ReadUInt16();
        Hue = pvSrc.ReadUInt16();
        Amount = pvSrc.ReadUInt16();
        Price = pvSrc.ReadUInt16();
        Name = pvSrc.ReadString(pvSrc.ReadUInt16());
    }
}