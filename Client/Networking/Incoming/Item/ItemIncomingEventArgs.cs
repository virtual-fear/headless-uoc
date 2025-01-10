namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class ItemIncomingEventArgs : EventArgs
    {
        // Might have to fix this, because we use it for MobileIncoming
        public NetState State { get; }
        public ItemIncomingEventArgs(NetState state) => State = state;
        public Serial Serial { get; set; }
        public int ItemID { get; set; }
        public Layer Layer { get; set; }
        public short Hue { get; set; }
    }
public partial class Item
{
    public static event PacketEventHandler<ItemIncomingEventArgs>? OnUpdateIncoming;
    internal static void RecevedItem_UpdateIncoming(NetState ns, PacketReader pvSrc)
    {
        ItemIncomingEventArgs e = new ItemIncomingEventArgs(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        if (e.Serial != 0x00)
        {
            e.ItemID = pvSrc.ReadUInt16();
            e.Layer = (Layer)pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
        }
        OnUpdateIncoming?.Invoke(e);
    }
}