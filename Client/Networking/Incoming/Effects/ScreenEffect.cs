namespace Client.Networking.Incoming.Effects;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ScreenEffectEventArgs>? UpdateScreenEffect;
    public sealed class ScreenEffectEventArgs : EventArgs
    {
        public NetState State { get; }
        public ScreenEffectEventArgs(NetState state) => State = state;
        public ScreenEffectType Type { get; set; }
    }

    protected static class ScreenEffect
    {
        [PacketHandler(0x70, length: 28, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            ScreenEffectEventArgs e = new(ns);
            if (pvSrc.ReadByte() != 0x04)
                pvSrc.Trace();
            pvSrc.ReadBytes(8);
            e.Type = (ScreenEffectType)pvSrc.ReadInt16();
            pvSrc.ReadBytes(16);
            UpdateScreenEffect?.Invoke(e);
        }
    }
}
