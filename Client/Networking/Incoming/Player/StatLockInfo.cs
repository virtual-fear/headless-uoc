namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<StatLockInfoEventArgs>? Player_StatLockInfo;                 // (ext) packetID: 0x19 (TODO-Fix: Same as bonded status?
    public sealed class StatLockInfoEventArgs : EventArgs
    {
        public NetState State { get; }
        public StatLockInfoEventArgs(NetState state) => State = state;
        public int Mobile { get; set; }

    }
    protected static class StatLockInfo
    {

        [PacketHandler(0x19, length: 12, ingame: true, extCmd: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            StatLockInfoEventArgs e = new StatLockInfoEventArgs(ns);
            pvSrc.ReadByte();
            e.Mobile = pvSrc.ReadInt32();
            pvSrc.ReadByte();
            byte lockBits = pvSrc.ReadByte();
            // TODO: Fix lockBits
            Player_StatLockInfo?.Invoke(e);
        }
    }
}
