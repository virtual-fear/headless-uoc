namespace Client.Networking.Incoming;
using Client.Game.Data;
public partial class Effect
{
    public static event PacketEventHandler<ScreenEffectEventArgs>? OnScreenUpdate;
    public sealed class ScreenEffectEventArgs : EventArgs
    {
        public NetState State { get; }
        public ScreenEffectEventArgs(NetState state) => State = state;
        public ScreenEffectType Type { get; set; }
    }

    [PacketHandler(0x70, length: 28, ingame: true)]
    protected static void Received_ScreenEffect(NetState ns, PacketReader pvSrc)
    {
        ScreenEffectEventArgs e = new(ns);
        if (pvSrc.ReadByte() != 0x04)
            pvSrc.Trace();
        pvSrc.ReadBytes(8);
        e.Type = (ScreenEffectType)pvSrc.ReadInt16();
        pvSrc.ReadBytes(16);
        OnScreenUpdate?.Invoke(e);
    }
}
