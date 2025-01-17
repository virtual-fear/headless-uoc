using PPing = Client.Networking.Outgoing.PPing;
namespace Client.Networking.Arguments;

using Client.Game;

/// <summary>
///     Server is requesting a ping.
/// </summary>
public sealed class PingReqEventArgs : EventArgs
{
    public NetState State { get; }
    public byte Value { get; }
    internal PingReqEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = ip.ReadByte();
    }

    static PingReqEventArgs() => Player.OnPingAck += Player_OnPingAck;
    private static void Player_OnPingAck(PingReqEventArgs e) => e.State.Send(PPing.Write(e.Value));
}