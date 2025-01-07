
namespace Client.Networking.Incoming;
public partial class PacketSink
{
    public sealed class UpdateStatueAnimationEventArgs : EventArgs
    {
        public NetState State { get; }
        public UpdateStatueAnimationEventArgs(NetState state) => State = state;
        public int Mobile { get; set; }
        public byte Status { get; set; }
        public byte Animation { get; set; }
        public byte Frame { get; set; }
    }

    public static event PacketEventHandler<UpdateStatueAnimationEventArgs>? UpdateStatueAnimation;
    public static void InvokeUpdateStatueAnimation(UpdateStatueAnimationEventArgs e) => UpdateStatueAnimation?.Invoke(e);
}

public static class UpdatedAnimations
{
    public static void Configure() => RegisterExtended(0x11, 17, true, new OnPacketReceive(StatueUpdate));
    private static void StatueUpdate(NetState ns, PacketReader pvSrc)
    {
        var e = new PacketSink.UpdateStatueAnimationEventArgs(ns);
        pvSrc.Seek(3, SeekOrigin.Current);
        //pvSrc.ReadInt16();  //  0x19
        //pvSrc.ReadByte();   //  0x05
        e.Mobile = pvSrc.ReadInt32();
        pvSrc.Seek(2, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        //pvSrc.ReadByte();   //  0xFF
        e.Status = pvSrc.ReadByte();
        pvSrc.Seek(1, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        e.Animation = pvSrc.ReadByte();
        pvSrc.Seek(1, SeekOrigin.Current);
        //pvSrc.ReadByte();   //  0x00
        e.Frame = pvSrc.ReadByte();
        PacketSink.InvokeUpdateStatueAnimation(e);
    }

    private static void RegisterExtended(byte packetID, int length, bool ingame, OnPacketReceive onReceive) => PacketHandlers.RegisterExtended(packetID, length, ingame, onReceive);
}
