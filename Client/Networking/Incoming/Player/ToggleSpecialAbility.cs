namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ToggleSpecialAbilityEventArgs>? Player_ToggleSpecialAbility; // (ext) packetID: 0x25
    public sealed class ToggleSpecialAbilityEventArgs : EventArgs
    {
        public NetState State { get; }
        public ToggleSpecialAbilityEventArgs(NetState state) => State = state;
        public short AbilityID { get; set; }
        public bool Active { get; set; }
    }
    protected static class ToggleSpecialAbility
    {
        [PacketHandler(0x25, length: 7, ingame: true, extCmd: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            ToggleSpecialAbilityEventArgs e = new(ns);
            e.AbilityID = pvSrc.ReadInt16();
            e.Active = pvSrc.ReadBoolean();
            Player_ToggleSpecialAbility?.Invoke(e);
        }
    }
}
