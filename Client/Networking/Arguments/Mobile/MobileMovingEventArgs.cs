namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MobileMovingEventArgs : EventArgs
{
    [PacketHandler(0x77, length: 17, ingame: true)]
    private static event PacketEventHandler<MobileMovingEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public short Body { get; }
    public IPoint3D Location { get; }
    public Direction Direction { get; }
    public short Hue { get; }
    public byte PacketFlags { get; }
    public Notoriety Notoriety { get; }
    private MobileMovingEventArgs(NetState state, PacketReader ip)
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
    }
    static MobileMovingEventArgs() => Update += MobileMovingEventArgs_Update;
    private static void MobileMovingEventArgs_Update(MobileMovingEventArgs e)
        => Mobile.Acquire(e.Serial).OnMove(e.State, e.Body, e.Hue, e.PacketFlags, e.Notoriety, e.Direction, e.Location);
}