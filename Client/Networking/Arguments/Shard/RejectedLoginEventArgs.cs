using Client.Game;

namespace Client.Networking.Arguments;
public sealed class RejectedLoginEventArgs : EventArgs
{
    [PacketHandler(0x53, length: -1, ingame: false)] // TODO: FIX ME!
    [PacketHandler(0x82, length: -1, ingame: false)] // may not get picked up by the register
    [PacketHandler(0x85, length: -1, ingame: false)] // bc we access CustomAttributes[0];
    private static event PacketEventHandler<RejectedLoginEventArgs>? Update = (e) => Logger.LogError($"Login rejected (0x{e.Command}:X2)");
    public NetState State { get; }
    public byte Command { get; }
    internal RejectedLoginEventArgs(NetState state, byte cmd)
    {
        State = state;
        Command = cmd;
    }

    static RejectedLoginEventArgs() => Update += RejectedLoginEventArgs_Update;
    private static void RejectedLoginEventArgs_Update(RejectedLoginEventArgs e) => Shard.Reject(e.State, e.Command);
}