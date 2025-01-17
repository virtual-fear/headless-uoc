namespace Client.Networking.Arguments;
public sealed class ToggleSpecialAbilityEventArgs : EventArgs
{
    public NetState State { get; }
    public short AbilityID { get; }
    public bool Active { get; }
    internal ToggleSpecialAbilityEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        AbilityID = ip.ReadInt16();
        Active = ip.ReadBoolean();
    }
}