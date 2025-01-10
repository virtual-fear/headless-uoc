namespace Client.Networking.Incoming.Display;
using Client.Game.Context;
public partial class PacketHandlers
{
    public static event PacketEventHandler<PaperdollEventArgs>? DisplayPaperdoll;

    public sealed class PaperdollEventArgs : EventArgs
    {
        public NetState State { get; }
        public PaperdollEventArgs(NetState state) => State = state;
        public MobileContext Mobile { get; set; }
        public string Text { get; set; }
        public bool Draggable { get; set; }
    }
    protected static class Paperdoll
    {
        [PacketHandler(0x88, length: 66, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            PaperdollEventArgs e = new PaperdollEventArgs(ns);
            e.Mobile = MobileContext.Acquire(pvSrc.ReadInt32());
            e.Text = pvSrc.ReadString(60);
            bool canLift = (pvSrc.ReadByte() & 2) != 0;
            e.Draggable = canLift;
            DisplayPaperdoll?.Invoke(e);
        }
    }
}