namespace Client.Networking.Incoming.Lighting;
public partial class PacketHandlers
{
    public static event PacketEventHandler<GlobalLightEventArgs>? UpdateGlobalLight;
    public sealed class GlobalLightEventArgs : EventArgs
    {
        public NetState State { get; }
        public GlobalLightEventArgs(NetState state) => State = state;
        public sbyte Level { get; set; }
    }
    protected static class GlobalLight
    {
        [PacketHandler(0x4F, length: 2, ingame: true)]
        private static void Update(NetState ns, PacketReader pvSrc)
        {
            GlobalLightEventArgs e = new(ns);
            e.Level = pvSrc.ReadSByte();
            UpdateGlobalLight?.Invoke(e);
        }
    }
}
