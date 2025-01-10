namespace Client.Networking.Incoming;
using Client.Game.Context;
using Client.Game.Data;
public sealed class PaperdollEventArgs : EventArgs
    {
        public NetState State { get; }
        public PaperdollEventArgs(NetState state) => State = state;
        public MobileContext Mobile { get; set; }
        public string Text { get; set; }
        public bool Draggable { get; set; }
    }
public partial class Display
{
    public static event PacketEventHandler<PaperdollEventArgs>? UpdatePaperdoll;

    [PacketHandler(0x88, length: 66, ingame: true)]
    protected static void ReceivedDisplay_Paperdoll(NetState ns, PacketReader pvSrc)
    {
        PaperdollEventArgs e = new PaperdollEventArgs(ns);
        e.Mobile = MobileContext.Acquire((Serial)pvSrc.ReadUInt32());
        e.Text = pvSrc.ReadString(60);
        bool canLift = (pvSrc.ReadByte() & 2) != 0;
        e.Draggable = canLift;
        UpdatePaperdoll?.Invoke(e);
    }
}