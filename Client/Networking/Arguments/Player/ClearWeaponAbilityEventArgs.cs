using Client.Game;

namespace Client.Networking.Arguments;
public sealed class ClearWeaponAbilityEventArgs : EventArgs
{
    [PacketHandler(0x21, length: 5, ingame: true, extCmd: true)]
    private static event PacketEventHandler<ClearWeaponAbilityEventArgs>? Update;
    public NetState State { get; }
    private ClearWeaponAbilityEventArgs(NetState state) => State = state;
    static ClearWeaponAbilityEventArgs() => Update += ClearWeaponAbilityEventArgs_Update;
    private static void ClearWeaponAbilityEventArgs_Update(ClearWeaponAbilityEventArgs e) => Player.ClearWeaponAbility(e.State);
}