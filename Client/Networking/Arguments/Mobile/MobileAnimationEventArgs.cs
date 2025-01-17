using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Arguments;
public sealed class MobileAnimationEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; }
    public int Action { get; }
    public int FrameCount { get; }
    public int RepeatCount { get; }
    public bool Forward { get; }
    public bool Repeat { get; }
    public byte Delay { get; }
    internal MobileAnimationEventArgs(NetState state, PacketReader ip)
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
}