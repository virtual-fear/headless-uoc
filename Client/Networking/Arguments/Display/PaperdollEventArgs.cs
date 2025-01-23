namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class PaperdollEventArgs : EventArgs
{
    [PacketHandler(0x88, length: 66, ingame: true)]
    private static event PacketEventHandler<PaperdollEventArgs> Update;
    public NetState State { get; }
    public Mobile Mobile { get; }
    public string Text { get; }
    public bool Draggable { get; }
    private PaperdollEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Mobile = Mobile.Acquire((Serial)pvSrc.ReadUInt32());
        Text = pvSrc.ReadString(60);
        bool canLift = (pvSrc.ReadByte() & 2) != 0;
        Draggable = canLift;
    }

    static PaperdollEventArgs() => Update += PaperdollEventArgs_Update;
    private static void PaperdollEventArgs_Update(PaperdollEventArgs e)
        => Display.ShowPaperdoll(e.State, e.Mobile, e.Text, e.Draggable);
}