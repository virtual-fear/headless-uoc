namespace Client.Networking.Incoming;
public sealed class ContainerDisplayEventArgs : EventArgs
{
    public NetState State { get; }
    public ContainerDisplayEventArgs(NetState state) => State = state;
    public int Container { get; set; }
    public short GumpID { get; set; }
}
public partial class Container
{
    public static event PacketEventHandler<ContainerDisplayEventArgs>? OnDisplay;
    [PacketHandler(0x24, length: 7, ingame: true)]
    protected static void ReceivedContainer_Display(NetState ns, PacketReader pvSrc)
    {
        ContainerDisplayEventArgs e = new(ns);
        e.Container = pvSrc.ReadInt32();
        e.GumpID = pvSrc.ReadInt16();
        OnDisplay?.Invoke(e);
    }
}