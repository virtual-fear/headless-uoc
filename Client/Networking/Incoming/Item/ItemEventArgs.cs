namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class ItemEventArgs : EventArgs
{
    public NetState State { get; }
    public ItemEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
    public short ItemID { get; set; }
    public short Amount { get; set; }
    public short X { get; set; }
    public short Y { get; set; }
    public sbyte Z { get; set; }
    public short Hue { get; set; }
    public byte Flags { get; set; }
}
public partial class Item
{
    public static event PacketEventHandler<ItemEventArgs>? OnUpdate;

    /// <summary>
    ///     <c>Note:</c> RunUO GMItemPacket is a duplicate of WorldItem, specifically for staff members.
    /// </summary>
    /// <param name="ns"></param>
    /// <param name="pvSrc"></param>
    [PacketHandler(0x1A, length: 20, ingame: true)]
    protected static void ReceivedItem_Update(NetState ns, PacketReader pvSrc)
    {
        ItemEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        e.ItemID = pvSrc.ReadInt16();
        e.Amount = pvSrc.ReadInt16();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadSByte();
        e.Hue = pvSrc.ReadInt16();
        e.Flags = pvSrc.ReadByte();
        OnUpdate?.Invoke(e);
    }
}