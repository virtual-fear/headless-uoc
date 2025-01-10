namespace Client.Networking.Incoming.Mobiles
{
    public partial class PacketHandlers
    {
        public static event PacketEventHandler<MobileAnimationEventArgs>? OnMobileAnimation;
        public sealed class MobileAnimationEventArgs : EventArgs
        {
            public NetState State { get; }
            public MobileAnimationEventArgs(NetState state) => State = state;
            public int Serial { get; set; }
            public int Action { get; set; }
            public int FrameCount { get; set; }
            public int RepeatCount { get; set; }
            public bool Forward { get; set; }
            public bool Repeat { get; set; }
            public byte Delay { get; set; }
        }
        protected static class MobileAnimation
        {
            [PacketHandler(0x6E, length: 14, ingame: true)]
            public static void Update(NetState ns, PacketReader pvSrc)
            {
                MobileAnimationEventArgs e = new MobileAnimationEventArgs(ns);

                e.Serial = pvSrc.ReadInt32();
                e.Action = pvSrc.ReadInt16();
                e.FrameCount = pvSrc.ReadInt16();
                e.RepeatCount = pvSrc.ReadInt16();
                e.Forward = pvSrc.ReadBoolean();
                e.Repeat = pvSrc.ReadBoolean();
                e.Delay = pvSrc.ReadByte();
                OnMobileAnimation?.Invoke(e);
            }
        }
    }
}