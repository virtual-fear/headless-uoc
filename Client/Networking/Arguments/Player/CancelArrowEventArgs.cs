using Client.Game;

namespace Client.Networking.Arguments;
public sealed class CancelArrowEventArgs : EventArgs
{
    [PacketHandler(0xBA, length: 6, ingame: true)]
    public static event PacketEventHandler<CancelArrowEventArgs>? Update;
    public NetState State { get; }
    internal CancelArrowEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.Seek(5, SeekOrigin.Begin);
    }

    static CancelArrowEventArgs() => Update += CancelArrowEventArgs_Update;
    private static void CancelArrowEventArgs_Update(CancelArrowEventArgs e) => Player.OnCancelArrow();
}