using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class MobileStamEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; }
    public short StamMax { get; }
    public short Stam { get; }
    internal MobileStamEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt16();
        StamMax = ip.ReadInt16();
        Stam = ip.ReadInt16();
    }
}