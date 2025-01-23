namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MobileIncomingEventArgs : EventArgs
{
    [PacketHandler(0x78, length: -1, ingame: true)]
    private static event PacketEventHandler<MobileIncomingEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public short Body { get; }
    public IPoint3D Location { get; }
    public Direction Direction { get; }
    public short Hue { get; }
    public byte PacketFlags { get; }
    public Notoriety Notoriety { get; }
    private MobileIncomingEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        Body = ip.ReadInt16();
        Location = new Point3D()
        {
            X = ip.ReadInt16(),
            Y = ip.ReadInt16(),
            Z = ip.ReadSByte()
        };
        Direction = (Direction)ip.ReadByte();
        Hue = ip.ReadInt16();
        PacketFlags = ip.ReadByte();
        Notoriety = (Notoriety)ip.ReadByte();
        //m.SetLocation( m.Parent, x, y, z );
        //Mobile m = ns.Mobile;
        //if (m.Player)
        //{
        //    m.Direction = (byte)(m.Direction & 7);
        //    m.Direction = (byte)(m.Direction | (m.Direction & 128));
        //}
        WorldItemIncomingEventArgs.Received_WorldIncomingItem(state, ip);
    }
    static MobileIncomingEventArgs() => Update += MobileIncomingEventArgs_Update;
    private static void MobileIncomingEventArgs_Update(MobileIncomingEventArgs e)
        => Mobile.Acquire(e.Serial).Update(e.State, e.Body, e.Hue, e.PacketFlags, e.Direction, e.Location, e.Notoriety);
}