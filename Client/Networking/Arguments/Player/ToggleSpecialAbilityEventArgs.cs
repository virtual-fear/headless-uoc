namespace Client.Networking.Arguments;
using Client.Game;
public sealed class ToggleSpecialAbilityEventArgs : EventArgs
{
    [PacketHandler(0x25, length: 7, ingame: true, extCmd: true)]
    public static event PacketEventHandler<ToggleSpecialAbilityEventArgs>? Update;
    public NetState State { get; }
    public short AbilityID { get; }
    public bool Active { get; }
    internal ToggleSpecialAbilityEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        AbilityID = ip.ReadInt16();
        Active = ip.ReadBoolean();
    }
    static ToggleSpecialAbilityEventArgs() => Update += ToggleSpecialAbilityEventArgs_Update;
    private static void ToggleSpecialAbilityEventArgs_Update(ToggleSpecialAbilityEventArgs e) => Player.SpecialAbility = e;
}