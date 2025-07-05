namespace Client.Networking.Arguments;
using Client.Game;
public sealed class ChangeCombatantEventArgs : EventArgs
{
    [PacketHandler(0xAA, length: 5, ingame: true)]
    private static event PacketEventHandler<ChangeCombatantEventArgs>? Update;
    public NetState State { get; }
    public int Serial { get; }
    private ChangeCombatantEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
    }
    static ChangeCombatantEventArgs() => Update += ChangeCombatantEventArgs_Update;
    private static void ChangeCombatantEventArgs_Update(ChangeCombatantEventArgs e) => Player.Combatant = e.Serial;
}