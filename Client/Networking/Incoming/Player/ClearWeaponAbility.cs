namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ClearWeaponAbilityEventArgs>? Player_ClearWeaponAbility;
    public sealed class ClearWeaponAbilityEventArgs : EventArgs
    {
        public NetState State { get; }
        public ClearWeaponAbilityEventArgs(NetState state) => State = state;
    }
    protected static class ClearWeaponAbility
    {
        [PacketHandler(0x21, length: 5, ingame: true, extCmd: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            ClearWeaponAbilityEventArgs e = new ClearWeaponAbilityEventArgs(ns);
            Player_ClearWeaponAbility?.Invoke(e);
        }
    }
}
