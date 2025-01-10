namespace Client.Networking.Incoming;
public sealed class SequenceEventArgs : EventArgs
{
    public NetState State { get; }
    public SequenceEventArgs(NetState state) => State = state;
    public int Value { get; set; }
}
public partial class Shard
{
    public static event PacketEventHandler<SequenceEventArgs>? SequenceUpdate;

    [PacketHandler(0x7B, length: 2, ingame: true)]
    protected static void Received_Sequence(NetState ns, PacketReader pvSrc)
    {
        SequenceEventArgs e = new(ns);
        e.Value = pvSrc.ReadByte();
        SequenceUpdate?.Invoke(e);
    }
}
