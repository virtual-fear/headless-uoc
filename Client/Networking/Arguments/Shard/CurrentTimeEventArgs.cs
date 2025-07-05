using Client.Game;

namespace Client.Networking.Arguments;
public sealed class CurrentTimeEventArgs : EventArgs
{
    [PacketHandler(0x5B, length: 4, ingame: false)]
    private static event PacketEventHandler<CurrentTimeEventArgs>? Update;

    public NetState State { get; }
    public TimeSpan Value { get; }
    internal CurrentTimeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = new TimeSpan(
                hours: ip.ReadByte(),
                minutes: ip.ReadByte(),
                seconds: ip.ReadByte());
    }

    static CurrentTimeEventArgs() => Update += CurrentTimeEventArgs_Update;
    private static void CurrentTimeEventArgs_Update(CurrentTimeEventArgs e) => Shard.CurrentTime = e.Value;
}