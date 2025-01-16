using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class MobileManaEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; set; }
    public short ManaMax { get; set; }
    public short Mana { get; set; }
    internal MobileManaEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        ManaMax = ip.ReadInt16();
        Mana = ip.ReadInt16();
    }
}