namespace Client.Game.Context;
using Client.Game.Data;
using Client.Networking.Incoming;
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
    static ItemContext() => Configure();
    private static void Configure()
    {
        Item.OnUpdate += Item_OnUpdate;
        Item.OnUpdateIncoming += Item_OnUpdateIncoming;
    }

    private static void ItemContext_OnRemove(RemoveEventArgs e)
    {
        Acquire(e.Serial)?.Delete();
    }

    private static void Item_OnUpdateIncoming(ItemIncomingEventArgs e)
    {
        var item = Acquire(e.Serial);
        item.ID = e.ItemID;
        item.Hue = e.Hue;
        item.Layer = e.Layer;
    }
    private static void Item_OnUpdate(ItemEventArgs e)
    {
        var item = Acquire(e.Serial);
        item.ID = e.ItemID;
        item.Hue = e.Hue;
        item.Amount = e.Amount;
        item.Flags = e.Flags;
        item.SetLocation(e.X, e.Y, e.Z);
    }
}