namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    [Obsolete]
    public static event PacketEventHandler<BondedStatusEventArgs>? Player_BondedStatus;
    
    [Obsolete]
    public sealed class BondedStatusEventArgs : EventArgs
    {
        public NetState State { get; }
        public BondedStatusEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public byte Value01 { get; set; }
        public byte Value02 { get; set; }
    }// (ext) packetID: 0x19

    [Obsolete]
    protected static class BondedStatus
    {
        [Obsolete("StatLockInfo is the same")]
        //[PacketHandler(0x19, length: 11, ingame: true, extCmd: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            BondedStatusEventArgs e = new BondedStatusEventArgs(ns);
            byte v1, v2;
            v1 = pvSrc.ReadByte();
            e.Serial = pvSrc.ReadInt32();
            v2 = pvSrc.ReadByte();
            e.Value01 = v1;
            e.Value02 = v2;
            Player_BondedStatus?.Invoke(e);
        }
    }
}
