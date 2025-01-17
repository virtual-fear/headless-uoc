using Notoriety = Client.Game.Data.Notoriety;
namespace Client.Networking.Arguments;
public sealed class MovementAckEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Sequence { get; }
    public Notoriety Notoriety { get; }
    internal MovementAckEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Sequence = pvSrc.ReadByte();
        Notoriety = (Notoriety)pvSrc.ReadByte();
    }
}