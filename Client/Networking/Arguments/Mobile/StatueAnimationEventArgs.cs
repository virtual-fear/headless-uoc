namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;

public sealed class StatueAnimationEventArgs : EventArgs
{
    [PacketHandler(0x11, length: 17, ingame: true, extCmd: true)]
    public static event PacketEventHandler<StatueAnimationEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public byte Status { get; }
    public byte Animation { get; }
    public byte Frame { get; }
    private StatueAnimationEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.Seek(3, SeekOrigin.Current);
        //pvSrc.ReadInt16();  //  0x19
        //pvSrc.ReadByte();   //  0x05
        Serial = (Serial)ip.ReadUInt32();
        ip.Seek(2, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        //pvSrc.ReadByte();   //  0xFF
        Status = ip.ReadByte();
        ip.Seek(1, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        Animation = ip.ReadByte();
        ip.Seek(1, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        Frame = ip.ReadByte();
    }
    static StatueAnimationEventArgs() => Update += StatueAnimationEventArgs_Update;
    private static void StatueAnimationEventArgs_Update(StatueAnimationEventArgs e)
        => Mobile.Acquire(e.Serial).UpdateAnimation(e.State, e.Status, e.Animation, e.Frame);
}