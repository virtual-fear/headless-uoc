namespace Client.Networking.Arguments;

[Obsolete]
public sealed class BondedStatusEventArgs : EventArgs
{
    //[PacketHandler(0x19, length: 11, ingame: true, extCmd: true)]
    //public static event PacketEventHandler<BondedStatusEventArgs>? Player_BondedStatus;
    public NetState State { get; }
    public int Serial { get; set; }
    public byte Value01 { get; set; }
    public byte Value02 { get; set; }
    internal BondedStatusEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        byte v1, v2;
        v1 = ip.ReadByte();
        Serial = ip.ReadInt32();
        v2 = ip.ReadByte();
        Value01 = v1;
        Value02 = v2;
    }
} // (ext) packetID: 0x19