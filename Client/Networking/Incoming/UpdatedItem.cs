using Client.Game.Context.Agents;
using Client.Game.Context.Data;
namespace Client.Networking.Incoming;

using static PacketSink;
public partial class PacketSink
{
    #region EventArgs
    public sealed class WorldItemEventArgs : EventArgs
    {
        public NetState State { get; }
        public WorldItemEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short ItemID { get; set; }
        public short Amount { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public sbyte Z { get; set; }
        public short Hue { get; set; }
        public byte Flags { get; set; }
    }
    public sealed class _WorldItemEventArgs : EventArgs
    {
        public NetState State { get; }
        public _WorldItemEventArgs(NetState state) => State = state;
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
    public sealed class WorldItemHSEventArgs : EventArgs
    {
        public NetState State { get; }
        public WorldItemHSEventArgs(NetState state) => State = state;
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
    public sealed class WorldItemSAEventArgs : EventArgs
    {
        public NetState State { get; }
        public WorldItemSAEventArgs(NetState state) => State = state;
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

    #endregion (done)

    public static event PacketEventHandler<WorldItemEventArgs>? WorldItem;
    public static event PacketEventHandler<WorldItemSAEventArgs>? WorldItemSA;
    public static event PacketEventHandler<WorldItemHSEventArgs>? WorldItemHS;
    public static event PacketEventHandler<ItemIncomingEventArgs>? ItemIncoming;
    public static void InvokeItemIncoming(ItemIncomingEventArgs e) => ItemIncoming?.Invoke(e);
    public static void InvokeWorldItemHS(WorldItemHSEventArgs e) => WorldItemHS?.Invoke(e);
    public static void InvokeWorldItemSA(WorldItemSAEventArgs e) => WorldItemSA?.Invoke(e);
    public static void InvokeWorldItem(WorldItemEventArgs e) => WorldItem?.Invoke(e);
}
public static class UpdatedItem
{
    public static void Configure()
    {
        Register(0x1A, 20, true, new OnPacketReceive(WorldItem)); // note: runuo:GMItemPacket is a duplicate of WorldItem specifically for: staff
        //Register(0xF3, 24, true, new OnPacketReceive(WorldItemSA));
        //Register(0xF3, 26, true, new OnPacketReceive(WorldItemHS));

        Item.Configure();
    }
    private static void WorldItemHS(NetState ns, PacketReader pvSrc)
    {
        WorldItemHSEventArgs e = new WorldItemHSEventArgs(ns);

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

        PacketSink.InvokeWorldItemHS(e);
    }
    private static void WorldItemSA(NetState ns, PacketReader pvSrc)
    {
        WorldItemSAEventArgs e = new WorldItemSAEventArgs(ns);

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

        PacketSink.InvokeWorldItemSA(e);
    }
    private static void WorldItem(NetState ns, PacketReader pvSrc)
    {
        WorldItemEventArgs e = new WorldItemEventArgs(ns);
        e.Serial = pvSrc.ReadInt32();
        e.ItemID = pvSrc.ReadInt16();
        e.Amount = pvSrc.ReadInt16();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadSByte();
        e.Hue = pvSrc.ReadInt16();
        e.Flags = pvSrc.ReadByte();
        PacketSink.InvokeWorldItem(e);
    }
    private static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive) => PacketHandlers.Register(packetID, length, variable, onReceive);
}
