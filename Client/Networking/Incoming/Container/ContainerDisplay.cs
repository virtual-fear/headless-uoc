namespace Client.Networking.Incoming.Container;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ContainerDisplayEventArgs>? Container_Display;
    public sealed class ContainerDisplayEventArgs : EventArgs
    {
        public NetState State { get; }
        public ContainerDisplayEventArgs(NetState state) => State = state;
        public int Container { get; set; }
        public short GumpID { get; set; }
    }
    protected static class ContainerDisplay
    {
        [PacketHandler(0x24, length: 7, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            ContainerDisplayEventArgs e = new(ns);
            e.Container = pvSrc.ReadInt32();
            e.GumpID = pvSrc.ReadInt16();
            Container_Display?.Invoke(e);
        }
    }
}
