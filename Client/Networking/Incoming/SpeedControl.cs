namespace Client.Networking.Incoming
{
    public partial class PacketHandlers
    {
        public static event PacketEventHandler<SpeedControlEventArgs>? UpdateSpeedControl;
        public sealed class SpeedControlEventArgs : EventArgs
        {
            public NetState State { get; }
            public SpeedControlEventArgs(NetState state) => State = state;
            public int Value { get; set; }
        }
        protected static class SpeedControl
        {
            [PacketHandler(0x26, length: 3, ingame: true, extCmd: true)]
            internal static void ExtendedUpdate(NetState ns, PacketReader pvSrc)
            {
                SpeedControlEventArgs e = new(ns);
                e.Value = pvSrc.ReadByte();
                UpdateSpeedControl?.Invoke(e);
            }
        }
    }
}
