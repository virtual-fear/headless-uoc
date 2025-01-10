namespace Client.Networking.Incoming.Items;
using Client.Game.Data;
public partial class PacketHandlers
{
    [Obsolete("use event: UpdateItem")]
    public static event PacketEventHandler<ItemHSEventArgs>? UpdateWorldItemHS;
    public sealed class ItemHSEventArgs : EventArgs
    {
        public NetState State { get; }
        public ItemHSEventArgs(NetState state) => State = state;
        public bool IsMulti { get; set; }
        public int Serial { get; set; }
        public short ItemID { get; set; }
        public short Amount { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public sbyte Z { get; set; }
        public LightType Light { get; set; }
        public short Hue { get; set; }
        public byte PacketFlags { get; set; }
    }
    protected static class ItemHS
    {
        [Obsolete]
        //[PacketHandler(0xF3, length: 26, ingame: true)]
        private static void Update(NetState ns, PacketReader pvSrc)
        {
            ItemHSEventArgs e = new(ns);
            pvSrc.Seek(2, SeekOrigin.Current);
            //pvSrc.ReadInt16();  //  0x01
            e.IsMulti = (pvSrc.ReadByte() == 0x02);
            e.Serial = pvSrc.ReadInt32();
            e.ItemID = pvSrc.ReadInt16();
            pvSrc.ReadByte();   //  0x00    :   terminate
            e.Amount = pvSrc.ReadInt16();
            e.Amount = pvSrc.ReadInt16();   //  (duplicate)
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Z = pvSrc.ReadSByte();
            e.Light = (LightType)pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
            e.PacketFlags = pvSrc.ReadByte();
            pvSrc.Seek(2, SeekOrigin.Current);
            //pvSrc.ReadInt16();  //  0x00
            UpdateWorldItemHS?.Invoke(e);
        }

    }
}
