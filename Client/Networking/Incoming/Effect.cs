using Client.Networking.Incoming.Effects;

namespace Client.Networking.Incoming;
public partial class Effect
{
    public static event PacketEventHandler<DragEffectEventArgs>? OnDragUpdate;
    public static event PacketEventHandler<HuedEffectEventArgs>? OnHuedUpdate;
    public static event PacketEventHandler<ParticleEffectEventArgs>? OnParticleUpdate;
    public static event PacketEventHandler<ScreenEffectEventArgs>? OnScreenUpdate;

    [PacketHandler(0x70, length: 28, ingame: true)]
    protected static void Received_ScreenEffect(NetState ns, PacketReader ip) => OnScreenUpdate?.Invoke(new(ns, ip));

    [PacketHandler(0xC7, length: 49, ingame: true)]
    protected static void Received_ParticleEffect(NetState ns, PacketReader ip) => OnParticleUpdate?.Invoke(new(ns, ip));

    [PacketHandler(0xC0, length: 36, ingame: true)]
    public static void Received_HuedEffect(NetState ns, PacketReader ip) => OnHuedUpdate?.Invoke(new(ns, ip));

    [PacketHandler(0x23, length: 26, ingame: true)]
    protected static void Received_DragEffect(NetState ns, PacketReader ip) => OnDragUpdate?.Invoke(new(ns, ip));
}
