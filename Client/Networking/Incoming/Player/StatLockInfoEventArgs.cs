namespace Client.Networking.Incoming;
public sealed class StatLockInfoEventArgs : EventArgs
{
    public NetState State { get; }
    public int Mobile { get; }
    internal StatLockInfoEventArgs(NetState state, PacketReader ip) {
        State = state;
        ip.ReadByte();
        Mobile = ip.ReadInt32();
        ip.ReadByte();
        byte lockBits = ip.ReadByte();
        // TODO: Fix lockBits
    }
}