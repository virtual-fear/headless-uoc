using MobileContext = Client.Game.Context.MobileContext;
using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Arguments;
public sealed class PaperdollEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileContext Mobile { get; }
    public string Text { get; }
    public bool Draggable { get; }
    internal PaperdollEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Mobile = MobileContext.Acquire((Serial)pvSrc.ReadUInt32());
        Text = pvSrc.ReadString(60);
        bool canLift = (pvSrc.ReadByte() & 2) != 0;
        Draggable = canLift;
    }
}