using Client.Game;

namespace Client.Networking.Arguments;
public sealed class ContainerContentUpdateEventArgs : EventArgs
{
    [PacketHandler(0x25, length: 20, ingame: true)]
    private static event PacketEventHandler<ContainerContentUpdateEventArgs>? Update;
    public NetState State { get; }
    public int Serial { get; }
    public ushort ID { get; }
    public ushort Amount { get; }
    public short X { get; }
    public short Y { get; }
    public int Parent { get; }
    public short Hue { get; }
    private ContainerContentUpdateEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Serial = pvSrc.ReadInt32();
        ID = pvSrc.ReadUInt16();
        pvSrc.ReadByte();   //  signed, itemID offset
        Amount = pvSrc.ReadUInt16();
        X = pvSrc.ReadInt16();
        Y = pvSrc.ReadInt16();
        Parent = pvSrc.ReadInt32();
        Hue = pvSrc.ReadInt16();
    }

    static ContainerContentUpdateEventArgs() => Update += Invoke;
    private static void Invoke(ContainerContentUpdateEventArgs e)
        => Container.DisplayContentUpdate(e.State, e.Serial, e.ID, e.Amount, e.X, e.Y, e.Parent, e.Hue);
}