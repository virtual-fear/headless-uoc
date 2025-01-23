namespace Client.Game;

using Client.Game.Data;
using Client.Networking.Arguments;

public interface IItemValidator
{
    bool IsValid(Item check);
}
public delegate void ItemEventHandler(Item item);
public class Item : Entity
{
    public static event ItemEventHandler? OnUpdate;
    public int ID { get; private set; }
    public short Hue { get; private set; }
    public short Amount { get; private set; }
    public byte Flags { get; private set; }
    public Layer Layer { get; private set; }
    public static Item Acquire(Serial serial) => World.WantItem(serial);
    public Item(Serial serial) : base(serial) { }
    internal void Update(WorldItemIncomingEventArgs e)
    {
        ID = e.ItemID;
        Layer = e.Layer;
        Hue = e.Hue;
        OnUpdate?.Invoke(this);
    }
    internal void Update(WorldItemEventArgs e)
    {
        ID = e.ItemID;
        Amount = e.Amount;
        Hue = e.Hue;
        Flags = e.Flags;
        Location = new Point3D()
        {
            X = e.X,
            Y = e.Y,
            Z = e.Z
        };
        OnUpdate?.Invoke(this);
    }
    static Item() => OnUpdate += Item_OnUpdate;
    private static void Item_OnUpdate(Item item) => item.PostUpdate();
    protected virtual void PostUpdate() { }
}