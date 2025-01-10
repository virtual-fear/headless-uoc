namespace Client.Networking.Incoming;
public sealed class ToggleSpecialAbilityEventArgs : EventArgs
{
    public NetState State { get; }
    public ToggleSpecialAbilityEventArgs(NetState state) => State = state;
    public short AbilityID { get; set; }
    public bool Active { get; set; }
}
public partial class Player
{
    public static event PacketEventHandler<ToggleSpecialAbilityEventArgs>? OnToggleSpecialAbility;

    [PacketHandler(0x25, length: 7, ingame: true, extCmd: true)]
    internal static void Receive_ToggleSpecialAbility(NetState ns, PacketReader pvSrc)
    {
        ToggleSpecialAbilityEventArgs e = new(ns);
        e.AbilityID = pvSrc.ReadInt16();
        e.Active = pvSrc.ReadBoolean();
        OnToggleSpecialAbility?.Invoke(e);
    }
}