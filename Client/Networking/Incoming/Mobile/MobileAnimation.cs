namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class MobileAnimationEventArgs : EventArgs
{
    public NetState State { get; }
    public MobileAnimationEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
    public int Action { get; set; }
    public int FrameCount { get; set; }
    public int RepeatCount { get; set; }
    public bool Forward { get; set; }
    public bool Repeat { get; set; }
    public byte Delay { get; set; }
}
public partial class Mobile
{
    public static event PacketEventHandler<MobileAnimationEventArgs>? OnAnimation;

    [PacketHandler(0x6E, length: 14, ingame: true)]
    protected static void Received_Animation(NetState ns, PacketReader pvSrc)
    {
        MobileAnimationEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        e.Action = pvSrc.ReadInt16();
        e.FrameCount = pvSrc.ReadInt16();
        e.RepeatCount = pvSrc.ReadInt16();
        e.Forward = pvSrc.ReadBoolean();
        e.Repeat = pvSrc.ReadBoolean();
        e.Delay = pvSrc.ReadByte();
        OnAnimation?.Invoke(e);
    }
}