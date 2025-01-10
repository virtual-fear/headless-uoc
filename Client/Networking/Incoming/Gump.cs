namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public static event PacketEventHandler<CloseGumpEventArgs> CloseGump;
    public class CloseGumpEventArgs : EventArgs
    {
        public NetState State { get; }
        public CloseGumpEventArgs(NetState state) => State = state;
        public int TypeID { get; set; }
        public int ButtonID { get; set; }
    }
    protected partial class Gump
    {
        [PacketHandler(0x04, length: 13, ingame: true, extCmd: true)]
        public static void Close(NetState ns, PacketReader pvSrc)
        {
            CloseGumpEventArgs e = new(ns);
            e.TypeID = pvSrc.ReadInt32();
            e.ButtonID = pvSrc.ReadInt32();
            PacketHandlers.CloseGump?.Invoke(e);
        }
    }
}
