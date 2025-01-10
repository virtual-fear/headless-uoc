namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MobileIncomingEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileIncomingEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
    public short Body { get; set; }
    public short X { get; set; }
    public short Y { get; set; }
    public sbyte Z { get; set; }
    public Direction Direction { get; set; }
    public short Hue { get; set; }
    public byte PacketFlags { get; set; }
    public Notoriety Notoriety { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<MobileIncomingEventArgs>? OnIncoming;

    [PacketHandler(0x78, length: -1, ingame: true)]
    protected static void Received_MobileIncoming(NetState ns, PacketReader pvSrc)
    {
        MobileIncomingEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        e.Body = pvSrc.ReadInt16();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadSByte();
        e.Direction = (Direction)pvSrc.ReadByte();
        e.Hue = pvSrc.ReadInt16();
        e.PacketFlags = pvSrc.ReadByte();
        e.Notoriety = (Notoriety)pvSrc.ReadByte();
        //m.SetLocation( m.Parent, x, y, z );
        //Mobile m = ns.Mobile;
        //if (m.Player)
        //{
        //    m.Direction = (byte)(m.Direction & 7);
        //    m.Direction = (byte)(m.Direction | (m.Direction & 128));
        //}
        Item.RecevedItem_UpdateIncoming(ns, pvSrc);
        OnIncoming?.Invoke(e);
    }
}