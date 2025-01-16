namespace Client.Networking.Incoming;
public sealed class FightingEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Flag { get; }
    public int Attacker { get; }
    public int Defender { get; }
    internal FightingEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Flag = ip.ReadByte();
        Attacker = ip.ReadInt32();
        Defender = ip.ReadInt32();
    }
}