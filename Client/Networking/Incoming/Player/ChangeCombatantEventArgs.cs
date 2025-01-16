namespace Client.Networking.Incoming;
public sealed class ChangeCombatantEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get; }
    internal ChangeCombatantEventArgs(NetState state, PacketReader ip) {
        State = state;
        Serial = ip.ReadInt32();
    }
}