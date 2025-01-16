using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class RemoveEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; }
    internal RemoveEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
    }
}