namespace Client.Networking.Incoming;
public sealed class ClearWeaponAbilityEventArgs : EventArgs
{
    public NetState State { get; }
    public ClearWeaponAbilityEventArgs(NetState state) => State = state;
}
public partial class Player
{
    public static event PacketEventHandler<ClearWeaponAbilityEventArgs>? OnClearWeaponAbility;

    [PacketHandler(0x21, length: 5, ingame: true, extCmd: true)]
    protected static void Receive_ClearWeaponAbility(NetState ns, PacketReader pvSrc)
    {
        ClearWeaponAbilityEventArgs e = new ClearWeaponAbilityEventArgs(ns);
        OnClearWeaponAbility?.Invoke(e);
    }
}