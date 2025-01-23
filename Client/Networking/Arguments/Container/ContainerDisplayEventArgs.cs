namespace Client.Networking.Arguments;
using Client.Game;
public sealed class ContainerDisplayEventArgs : EventArgs
{
    [PacketHandler(0x24, length: 7, ingame: true)]
    private static event PacketEventHandler<ContainerDisplayEventArgs>? Update;
    public NetState State { get; }
    public int ContainerID { get; }
    public short GumpID { get; }
    private ContainerDisplayEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ContainerID = ip.ReadInt32();
        GumpID = ip.ReadInt16();
    }
    static ContainerDisplayEventArgs() => Update += ContainerDisplayEventArgs_Update;
    private static void ContainerDisplayEventArgs_Update(ContainerDisplayEventArgs e)
        => Container.Display(e.State, e.ContainerID, e.GumpID);
}