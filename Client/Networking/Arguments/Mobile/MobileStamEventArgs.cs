namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MobileStamEventArgs : EventArgs
{
    [PacketHandler(0xA3, length: 9, ingame: true)]
    private static event PacketEventHandler<MobileStamEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public short ValueMax { get; }
    public short Value { get; }
    private MobileStamEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt16();
        ValueMax = ip.ReadInt16();
        Value = ip.ReadInt16();
    }

    static MobileStamEventArgs() => Update += MobileStamEventArgs_Update;
    private static void MobileStamEventArgs_Update(MobileStamEventArgs e)
        => Mobile.Acquire(e.Serial).UpdateStamina(e.State, e.Value, e.ValueMax);
}