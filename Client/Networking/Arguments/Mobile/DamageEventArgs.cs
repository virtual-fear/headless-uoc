using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Arguments;
public sealed class DamageEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Mobile { get; set; }
    public ushort Amount { get; set; }
    internal DamageEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadByte();
        Mobile = (Serial)ip.ReadUInt32();
        Amount = ip.ReadByte();
    }
}