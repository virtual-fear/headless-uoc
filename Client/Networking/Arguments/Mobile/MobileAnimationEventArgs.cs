namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MobileAnimationEventArgs : EventArgs
{
    [PacketHandler(0x6E, length: 14, ingame: true)]
    private static event PacketEventHandler<MobileAnimationEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public int Action { get; }
    public int FrameCount { get; }
    public int RepeatCount { get; }
    public bool Forward { get; }
    public bool Repeat { get; }
    public byte Delay { get; }
    private MobileAnimationEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        Action = ip.ReadInt16();
        FrameCount = ip.ReadInt16();
        RepeatCount = ip.ReadInt16();
        Forward = ip.ReadBoolean();
        Repeat = ip.ReadBoolean();
        Delay = ip.ReadByte();
    }
    static MobileAnimationEventArgs() => Update += MobileAnimationEventArgs_Update;
    private static void MobileAnimationEventArgs_Update(MobileAnimationEventArgs e)
        => Mobile.Acquire(e.Serial).Update(e);
}