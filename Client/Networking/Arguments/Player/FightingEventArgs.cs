namespace Client.Networking.Arguments;
using Client.Game;
public sealed class FightingEventArgs : EventArgs
{
    [PacketHandler(0x2F, length: 10, ingame: true)] // RunUO: Swing
    private static event PacketEventHandler<FightingEventArgs>? Update;
    public NetState State { get; }
    public byte Flag { get; }
    public int Attacker { get; }
    public int Defender { get; }
    private FightingEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Flag = ip.ReadByte();
        Attacker = ip.ReadInt32();
        Defender = ip.ReadInt32();
    }
    static FightingEventArgs() => Update += FightingEventArgs_Update;
    private static void FightingEventArgs_Update(FightingEventArgs e) => Player.OnFight(e.Flag, e.Attacker, e.Defender);
}