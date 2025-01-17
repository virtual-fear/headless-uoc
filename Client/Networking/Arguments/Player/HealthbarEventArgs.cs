using HealthbarType = Client.Game.Data.HealthbarType;
namespace Client.Networking.Arguments;
public sealed class HealthbarEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get; }
    public HealthbarType Type { get; }
    public byte Level { get; }
    internal HealthbarEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
        ip.Seek(2, SeekOrigin.Current);
        Type = (HealthbarType)ip.ReadInt16();
        Level = ip.ReadByte();
    }
}