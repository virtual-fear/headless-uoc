namespace Client.Networking.Arguments;
using Client.Game;
public sealed class SequenceEventArgs : EventArgs
{
    [PacketHandler(0x7B, length: 2, ingame: true)]
    private static event PacketEventHandler<SequenceEventArgs>? Update;
    public NetState State { get; }
    public int Value { get; }
    internal SequenceEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = ip.ReadByte();
    }
    static SequenceEventArgs() => Update += SequenceEventArgs_Update;
    private static void SequenceEventArgs_Update(SequenceEventArgs e) => Shard.Sequence = e.Value;
}