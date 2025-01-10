namespace Client.Networking.Incoming;
public sealed class SoundEventArgs : EventArgs
{
    public NetState State { get; }
    public SoundEventArgs(NetState state) => State = state;
    public short SoundID { get; set; }
    public short X { get; set; }
    public short Y { get; set; }
    public short Z { get; set; }
}
public partial class World
{
    public static event PacketEventHandler<SoundEventArgs>? UpdateSound;

    [PacketHandler(0x54, length: 12, ingame: true)]
    protected static void Received_Sound(NetState ns, PacketReader pvSrc)
    {
        SoundEventArgs e = new SoundEventArgs(ns);
        pvSrc.ReadByte();   //  Flags
        e.SoundID = pvSrc.ReadInt16();
        pvSrc.ReadByte();   //  Volume
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadInt16();
        UpdateSound?.Invoke(e);
    }
}