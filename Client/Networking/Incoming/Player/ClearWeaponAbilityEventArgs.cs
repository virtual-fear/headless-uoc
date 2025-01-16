namespace Client.Networking.Incoming;
public sealed class ClearWeaponAbilityEventArgs : EventArgs
{
    public NetState State { get; }
    internal ClearWeaponAbilityEventArgs(NetState state) => State = state;
}