namespace Client.Networking.Incoming.Items;
using Client.Game.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ItemEventArgs>? OnItemUpdate;
    public static event PacketEventHandler<ItemIncomingEventArgs>? OnItemIncoming;
    public sealed class ItemEventArgs : EventArgs
    {
        public NetState State { get; }
        public ItemEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short ItemID { get; set; }
        public short Amount { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public sbyte Z { get; set; }
        public short Hue { get; set; }
        public byte Flags { get; set; }
    }
    public sealed class ItemIncomingEventArgs : EventArgs
    {
        // Might have to fix this, because we use it for MobileIncoming
        public NetState State { get; }
        public ItemIncomingEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public int ItemID { get; set; }
        public Layer Layer { get; set; }
        public short Hue { get; set; }
    }
    public partial class Item
    {
        static Item() => Game.Context.ItemContext.Configure();

        /// <summary>
        ///     <c>Note:</c> RunUO GMItemPacket is a duplicate of WorldItem, specifically for staff members.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="pvSrc"></param>
        [PacketHandler(0x1A, length: 20, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            ItemEventArgs e = new(ns);
            e.Serial = pvSrc.ReadInt32();
            e.ItemID = pvSrc.ReadInt16();
            e.Amount = pvSrc.ReadInt16();
            e.X = pvSrc.ReadInt16();
            e.Y = pvSrc.ReadInt16();
            e.Z = pvSrc.ReadSByte();
            e.Hue = pvSrc.ReadInt16();
            e.Flags = pvSrc.ReadByte();
            OnItemUpdate?.Invoke(e);
        }

        public static void UpdateIncoming(NetState ns, PacketReader pvSrc)
        {
            ItemIncomingEventArgs e = new ItemIncomingEventArgs(ns);
            e.Serial = pvSrc.ReadInt32();
            if (e.Serial != 0x00)
            {
                e.ItemID = pvSrc.ReadUInt16();
                e.Layer = (Layer)pvSrc.ReadByte();
                e.Hue = pvSrc.ReadInt16();
            }
            OnItemIncoming?.Invoke(e);
        }
    }
}