namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public static event PacketEventHandler<PauseEventArgs>? OnPause;
    public sealed class PauseEventArgs : EventArgs
    {
        public NetState State { get; }
        public bool Supported { get; }
        public PauseEventArgs(NetState state, bool supported)
        {
            State = state;
            Supported = supported;
        }
    }
    protected static class Pause
    {
        [PacketHandler(0x33, 02, true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            PauseEventArgs e = new(ns, false);
            OnPause?.Invoke(e);
        }
    }
}
