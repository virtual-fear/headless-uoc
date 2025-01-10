namespace Client.Networking.Incoming;
public sealed class StatLockInfoEventArgs : EventArgs
{
    public NetState State { get; }
    public StatLockInfoEventArgs(NetState state) => State = state;
    public int Mobile { get; set; }

}
public partial class Player
{
    public static event PacketEventHandler<StatLockInfoEventArgs>? UpdateStatLockInfo; // (ext) packetID: 0x19 (TODO-Fix: Same as bonded status?

    [PacketHandler(0x19, length: 12, ingame: true, extCmd: true)]
    protected static void Receive_StatLockInfo(NetState ns, PacketReader pvSrc)
    {
        StatLockInfoEventArgs e = new StatLockInfoEventArgs(ns);
        pvSrc.ReadByte();
        e.Mobile = pvSrc.ReadInt32();
        pvSrc.ReadByte();
        byte lockBits = pvSrc.ReadByte();
        // TODO: Fix lockBits
        UpdateStatLockInfo?.Invoke(e);
    }
}