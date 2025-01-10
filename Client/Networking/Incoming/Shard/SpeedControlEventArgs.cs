namespace Client.Networking.Incoming;
public sealed class SpeedControlEventArgs : EventArgs
{
    public NetState State { get; }
    public SpeedControlEventArgs(NetState state) => State = state;
    public int Value { get; set; }
}
public partial class Shard
{
    public static event PacketEventHandler<SpeedControlEventArgs>? SpeedControlUpdate;

    [PacketHandler(0x26, length: 3, ingame: true, extCmd: true)]
    protected static void ExtendedUpdate(NetState ns, PacketReader pvSrc)
    {
        SpeedControlEventArgs e = new(ns);
        e.Value = pvSrc.ReadByte();
        SpeedControlUpdate?.Invoke(e);
    }
}
