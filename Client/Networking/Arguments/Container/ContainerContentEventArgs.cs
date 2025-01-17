using ContainerItem = Client.Game.Data.ContainerItem;
namespace Client.Networking.Arguments;
public sealed class ContainerContentEventArgs : EventArgs
{
    public NetState State { get; }
    public ContainerItem[]? Items { get; }
    internal ContainerContentEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        ContainerItem[] items = new ContainerItem[pvSrc.ReadUInt16()];
        for (int i = 0; i < items.Length; ++i)
        {
            ContainerItem ci = new ContainerItem(state);
            ci.Serial = pvSrc.ReadInt32();
            ci.ID = pvSrc.ReadUInt16();
            pvSrc.ReadByte();   //  0   :   signed, itemID offset
            ci.Amount = pvSrc.ReadUInt16();
            ci.X = pvSrc.ReadInt16();
            ci.Y = pvSrc.ReadInt16();
            ci.Parent = pvSrc.ReadInt32();
            ci.Hue = pvSrc.ReadUInt16();
            items[i] = ci;
        }
        Items = items;
    }
}