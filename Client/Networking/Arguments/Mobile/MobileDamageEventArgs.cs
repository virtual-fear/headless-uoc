using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Arguments;
public sealed class MobileDamageEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; set; }
    public ushort Amount { get; set; }
    internal MobileDamageEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        Amount = ip.ReadUInt16();
    }
}