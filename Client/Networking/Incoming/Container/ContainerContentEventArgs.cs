using Client.Game.Data;

namespace Client.Networking.Incoming;
public sealed class ContainerContentEventArgs : EventArgs
{
    public NetState State { get; }
    public ContainerContentEventArgs(NetState state) => State = state;
    public ContainerItem[]? Items { get; set; }
}
public partial class Container
{
    public static event PacketEventHandler<ContainerContentEventArgs>? OnContent;

    [PacketHandler(0x3C, length: -1, ingame: true)]
    protected static void ReceivedContainer_Content(NetState ns, PacketReader pvSrc)
    {
        ContainerContentEventArgs e = new(ns);
        ContainerItem[] items = new ContainerItem[pvSrc.ReadUInt16()];
        for (int i = 0; i < items.Length; ++i)
        {
            ContainerItem ci = new ContainerItem(ns);
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
        e.Items = items;
        OnContent?.Invoke(e);
    }

}
