using Client.Game;
namespace Client.Networking.Arguments;
public sealed class WorldPauseEventArgs : EventArgs
{
    [PacketHandler(0x33, length: 2, ingame: true)]
    private static event PacketEventHandler<WorldPauseEventArgs>? Update;
    public NetState State { get; }
    public bool Supported { get; } = false;
    internal WorldPauseEventArgs(NetState state, PacketReader ip) => State = state;
    static WorldPauseEventArgs() => Update += WorldPauseEventArgs_Update;
    private static void WorldPauseEventArgs_Update(WorldPauseEventArgs e) => World.Pause(e.State);
}