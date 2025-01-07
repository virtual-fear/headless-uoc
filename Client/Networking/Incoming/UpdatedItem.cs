using Client.Game;
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

    public static event PacketEventHandler<WorldItemEventArgs> WorldItem;
    public static event PacketEventHandler<WorldItemSAEventArgs> WorldItemSA;
    public static event PacketEventHandler<WorldItemHSEventArgs> WorldItemHS;
    public static event PacketEventHandler<ItemIncomingEventArgs> ItemIncoming;
    public static void InvokeItemIncoming(ItemIncomingEventArgs e) => ItemIncoming?.Invoke(e);
    public static void InvokeWorldItemHS(WorldItemHSEventArgs e) => WorldItemHS?.Invoke(e);
    public static void InvokeWorldItemSA(WorldItemSAEventArgs e) => WorldItemSA?.Invoke(e);
    public static void InvokeWorldItem(WorldItemEventArgs e) => WorldItem?.Invoke(e);
}

/// <summary>
/// Enumeration containing all possible light types. These are only applicable to light source items, like lanterns, candles, braziers, etc.
/// </summary>
public enum LightType
{
    /// <summary>
    /// Window shape, arched, ray shining east.
    /// </summary>
    ArchedWindowEast,
    /// <summary>
    /// Medium circular shape.
    /// </summary>
    Circle225,
    /// <summary>
    /// Small circular shape.
    /// </summary>
    Circle150,
    /// <summary>
    /// Door shape, shining south.
    /// </summary>
    DoorSouth,
    /// <summary>
    /// Door shape, shining east.
    /// </summary>
    DoorEast,
    /// <summary>
    /// Large semicircular shape (180 degrees), north wall.
    /// </summary>
    NorthBig,
    /// <summary>
    /// Large pie shape (90 degrees), north-east corner.
    /// </summary>
    NorthEastBig,
    /// <summary>
    /// Large semicircular shape (180 degrees), east wall.
    /// </summary>
    EastBig,
    /// <summary>
    /// Large semicircular shape (180 degrees), west wall.
    /// </summary>
    WestBig,
    /// <summary>
    /// Large pie shape (90 degrees), south-west corner.
    /// </summary>
    SouthWestBig,
    /// <summary>
    /// Large semicircular shape (180 degrees), south wall.
    /// </summary>
    SouthBig,
    /// <summary>
    /// Medium semicircular shape (180 degrees), north wall.
    /// </summary>
    NorthSmall,
    /// <summary>
    /// Medium pie shape (90 degrees), north-east corner.
    /// </summary>
    NorthEastSmall,
    /// <summary>
    /// Medium semicircular shape (180 degrees), east wall.
    /// </summary>
    EastSmall,
    /// <summary>
    /// Medium semicircular shape (180 degrees), west wall.
    /// </summary>
    WestSmall,
    /// <summary>
    /// Medium semicircular shape (180 degrees), south wall.
    /// </summary>
    SouthSmall,
    /// <summary>
    /// Shaped like a wall decoration, north wall.
    /// </summary>
    DecorationNorth,
    /// <summary>
    /// Shaped like a wall decoration, north-east corner.
    /// </summary>
    DecorationNorthEast,
    /// <summary>
    /// Small semicircular shape (180 degrees), east wall.
    /// </summary>
    EastTiny,
    /// <summary>
    /// Shaped like a wall decoration, west wall.
    /// </summary>
    DecorationWest,
    /// <summary>
    /// Shaped like a wall decoration, south-west corner.
    /// </summary>
    DecorationSouthWest,
    /// <summary>
    /// Small semicircular shape (180 degrees), south wall.
    /// </summary>
    SouthTiny,
    /// <summary>
    /// Window shape, rectangular, no ray, shining south.
    /// </summary>
    RectWindowSouthNoRay,
    /// <summary>
    /// Window shape, rectangular, no ray, shining east.
    /// </summary>
    RectWindowEastNoRay,
    /// <summary>
    /// Window shape, rectangular, ray shining south.
    /// </summary>
    RectWindowSouth,
    /// <summary>
    /// Window shape, rectangular, ray shining east.
    /// </summary>
    RectWindowEast,
    /// <summary>
    /// Window shape, arched, no ray, shining south.
    /// </summary>
    ArchedWindowSouthNoRay,
    /// <summary>
    /// Window shape, arched, no ray, shining east.
    /// </summary>
    ArchedWindowEastNoRay,
    /// <summary>
    /// Window shape, arched, ray shining south.
    /// </summary>
    ArchedWindowSouth,
    /// <summary>
    /// Large circular shape.
    /// </summary>
    Circle300,
    /// <summary>
    /// Large pie shape (90 degrees), north-west corner.
    /// </summary>
    NorthWestBig,
    /// <summary>
    /// Negative light. Medium pie shape (90 degrees), south-east corner.
    /// </summary>
    DarkSouthEast,
    /// <summary>
    /// Negative light. Medium semicircular shape (180 degrees), south wall.
    /// </summary>
    DarkSouth,
    /// <summary>
    /// Negative light. Medium pie shape (90 degrees), north-west corner.
    /// </summary>
    DarkNorthWest,
    /// <summary>
    /// Negative light. Medium pie shape (90 degrees), south-east corner. Equivalent to <c>LightType.SouthEast</c>.
    /// </summary>
    DarkSouthEast2,
    /// <summary>
    /// Negative light. Medium circular shape (180 degrees), east wall.
    /// </summary>
    DarkEast,
    /// <summary>
    /// Negative light. Large circular shape.
    /// </summary>
    DarkCircle300,
    /// <summary>
    /// Opened door shape, shining south.
    /// </summary>
    DoorOpenSouth,
    /// <summary>
    /// Opened door shape, shining east.
    /// </summary>
    DoorOpenEast,
    /// <summary>
    /// Window shape, square, ray shining east.
    /// </summary>
    SquareWindowEast,
    /// <summary>
    /// Window shape, square, no ray, shining east.
    /// </summary>
    SquareWindowEastNoRay,
    /// <summary>
    /// Window shape, square, ray shining south.
    /// </summary>
    SquareWindowSouth,
    /// <summary>
    /// Window shape, square, no ray, shining south.
    /// </summary>
    SquareWindowSouthNoRay,
    /// <summary>
    /// Empty.
    /// </summary>
    Empty,
    /// <summary>
    /// Window shape, skinny, no ray, shining south.
    /// </summary>
    SkinnyWindowSouthNoRay,
    /// <summary>
    /// Window shape, skinny, ray shining east.
    /// </summary>
    SkinnyWindowEast,
    /// <summary>
    /// Window shape, skinny, no ray, shining east.
    /// </summary>
    SkinnyWindowEastNoRay,
    /// <summary>
    /// Shaped like a hole, shining south.
    /// </summary>
    HoleSouth,
    /// <summary>
    /// Shaped like a hole, shining south.
    /// </summary>
    HoleEast,
    /// <summary>
    /// Large circular shape with a moongate graphic embeded.
    /// </summary>
    Moongate,
    /// <summary>
    /// Unknown usage. Many rows of slightly angled lines.
    /// </summary>
    Strips,
    /// <summary>
    /// Shaped like a small hole, shining south.
    /// </summary>
    SmallHoleSouth,
    /// <summary>
    /// Shaped like a small hole, shining east.
    /// </summary>
    SmallHoleEast,
    /// <summary>
    /// Large semicircular shape (180 degrees), north wall. Identical graphic as <c>LightType.NorthBig</c>, but slightly different positioning.
    /// </summary>
    NorthBig2,
    /// <summary>
    /// Large semicircular shape (180 degrees), west wall. Identical graphic as <c>LightType.WestBig</c>, but slightly different positioning.
    /// </summary>
    WestBig2,
    /// <summary>
    /// Large pie shape (90 degrees), north-west corner. Equivalent to <c>LightType.NorthWestBig</c>.
    /// </summary>
    NorthWestBig2
}

public static class UpdatedItem
{
    public static void Configure()
    {
        Register(0x1A, 20, true, new OnPacketReceive(WorldItem)); // note: runuo:GMItemPacket is a duplicate of WorldItem specifically for: staff
        //Register(0xF3, 24, true, new OnPacketReceive(WorldItemSA));
        //Register(0xF3, 26, true, new OnPacketReceive(WorldItemHS));

        Game.Item.Configure();
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
