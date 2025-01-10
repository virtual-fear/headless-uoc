namespace Client.Game.Context;
using Client.Game.Data;
using static Client.Networking.Incoming.Items.PacketHandlers;
using static Client.Networking.Incoming.PacketHandlers;
public interface IItemValidator
{
    bool IsValid(ItemContext check);
}
public sealed class ItemContext : ContextEntity
{
    public int ID { get; private set; }
    public short Hue { get; private set; }
    public short Amount { get; private set; }
    public byte Flags { get; private set; }
    public Layer Layer { get; private set; }
    public static ItemContext Acquire(Serial serial) => WorldContext.WantItem(serial);
    public ItemContext(Serial serial) : base(serial) { }
    public static void Configure()
    {
        OnItemIncoming += ItemContext_OnItemIncoming;
        OnItemUpdate += ItemContext_OnItemUpdate;
        OnRemove += ItemContext_OnRemove;
    }
    private static void ItemContext_OnRemove(RemoveEventArgs e)
    {
        Acquire(e.Serial)?.Delete();
    }
    private static void ItemContext_OnItemUpdate(ItemEventArgs e)
    {
        var item = Acquire(e.Serial);
        item.ID = e.ItemID;
        item.Hue = e.Hue;
        item.Amount = e.Amount;
        item.Flags = e.Flags;
        item.SetLocation(e.X, e.Y, e.Z);
    }
    private static void ItemContext_OnItemIncoming(ItemIncomingEventArgs e)
    {
        var item = Acquire(e.Serial);
        item.ID = e.ItemID;
        item.Hue = e.Hue;
        item.Layer = e.Layer;
    }
}