using Client.Game;
using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Arguments;
public sealed class DamageEventArgs : EventArgs
{
    [PacketHandler(0x22, length: 11, ingame: true, extCmd: true)]
    private static event PacketEventHandler<DamageEventArgs>? Update;
    public NetState State { get; }
    public Serial Mobile { get; set; }
    public ushort Amount { get; set; }
    private DamageEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadByte();
        Mobile = (Serial)ip.ReadUInt32();
        Amount = ip.ReadByte();
    }
    static DamageEventArgs() => Update += DamageEventArgs_OnUpdate;
    private static void DamageEventArgs_OnUpdate(DamageEventArgs e) => World.WantMobile(serial: e.Mobile).Update(e);
}