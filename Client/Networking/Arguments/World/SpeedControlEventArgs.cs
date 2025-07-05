namespace Client.Networking.Arguments;
using Client.Game;
public sealed class SpeedControlEventArgs : EventArgs
{
    [PacketHandler(0x26, length: 3, ingame: true, extCmd: true)]
    private static event PacketEventHandler<SpeedControlEventArgs>? Update;
    public NetState State { get; }
    public int Value { get; }
    private SpeedControlEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Value = ip.ReadByte();
    }
    static SpeedControlEventArgs() => Update += SpeedControlEventArgs_Update;
    private static void SpeedControlEventArgs_Update(SpeedControlEventArgs e) => World.SpeedControl.Value = e.Value;
}