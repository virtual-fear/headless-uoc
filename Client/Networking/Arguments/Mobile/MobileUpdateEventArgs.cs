namespace Client.Networking.Arguments;

using Client.Game;
using Client.Game.Data;
public sealed class MobileUpdateEventArgs : EventArgs
{
    [PacketHandler(0x20, length: 19, ingame: true)]
    public static event PacketEventHandler<MobileUpdateEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public short Body { get; }
    public short Hue { get; }
    public byte PacketFlags { get; }
    public Direction Direction { get; }
    public IPoint3D Location { get; }
    internal MobileUpdateEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        Body = (short)ip.ReadUInt16();
        ip.ReadByte();   //  0
        Hue = ip.ReadInt16();
        PacketFlags = ip.ReadByte();
        var x = ip.ReadInt16();
        var y = ip.ReadInt16();
        ip.ReadInt16();  //  0
        Direction = (Direction)ip.ReadByte();
        var z = ip.ReadSByte();
        Location = new Point3D() { X = x, Y = y, Z = z };
    }
    static MobileUpdateEventArgs() => Update += MobileUpdateEventArgs_Update;
    private static void MobileUpdateEventArgs_Update(MobileUpdateEventArgs e)
         => Mobile.Acquire(e.Serial).Update(e.State, e.Body, e.Hue, e.PacketFlags, e.Direction, e.Location);
}