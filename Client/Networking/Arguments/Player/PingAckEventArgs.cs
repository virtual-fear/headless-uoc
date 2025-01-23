namespace Client.Networking.Arguments;
using Client.Networking.Packets;

/// <summary>
///     Server is requesting a ping.
/// </summary>
public sealed class PingReqEventArgs : EventArgs
{
    [PacketHandler(0x73, length: 2, ingame: true)]
    private static event PacketEventHandler<PingReqEventArgs>? Update;
    public NetState State { get; }
    public byte Value { get; }
    private PingReqEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = ip.ReadByte();
    }
    static PingReqEventArgs() => Update += PingReqEventArgs_Update;
    private static void PingReqEventArgs_Update(PingReqEventArgs e) => e.State.Send(PPing.Write(e.Value));
}