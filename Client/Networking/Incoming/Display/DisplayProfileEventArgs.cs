using MobileContext = Client.Game.Context.MobileContext;
using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class DisplayProfileEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileContext? Mobile { get; }
    public string? Header { get; }
    public string? Footer { get; }
    public string? Body { get; }
    internal DisplayProfileEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Mobile = MobileContext.Acquire((Serial)ip.ReadUInt32());
        Header = ip.ReadString();
        Footer = ip.ReadUnicodeString();
        Body = ip.ReadUnicodeString();
    }
}