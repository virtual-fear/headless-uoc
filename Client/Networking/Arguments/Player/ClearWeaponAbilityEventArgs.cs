namespace Client.Networking.Arguments;
public sealed class ClearWeaponAbilityEventArgs : EventArgs
{
    public NetState State { get; }
    internal ClearWeaponAbilityEventArgs(NetState state) => State = state;
}