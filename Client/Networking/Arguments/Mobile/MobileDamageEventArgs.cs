namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MobileDamageEventArgs : EventArgs
{
    [PacketHandler(0x0B, length: 7, ingame: true)]
    private static event PacketEventHandler<MobileDamageEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; set; }
    public ushort Amount { get; set; }
    private MobileDamageEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        Amount = ip.ReadUInt16();
    }
    static MobileDamageEventArgs() => Update += MobileDamageEventArgs_Update;
    private static void MobileDamageEventArgs_Update(MobileDamageEventArgs e)
        => Mobile.Acquire(e.Serial).Hits -= e.Amount;
}