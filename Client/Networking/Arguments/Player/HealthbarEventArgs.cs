namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class HealthbarEventArgs : EventArgs
{
    [PacketHandler(0x17, length: 12, ingame: true)]
    private static event PacketEventHandler<HealthbarEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public HealthbarType Type { get; }
    public byte Level { get; }
    private HealthbarEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        ip.Seek(2, SeekOrigin.Current);
        Type = (HealthbarType)ip.ReadInt16();
        Level = ip.ReadByte();
    }

    static HealthbarEventArgs() => Update += HealthbarEventArgs_Update;
    private static void HealthbarEventArgs_Update(HealthbarEventArgs e) 
        => Player.UpdateHealthbar(e.State, Mobile.Acquire(e.Serial), e.Type, e.Level);
}