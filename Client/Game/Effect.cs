namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public partial class Effect
{
    [PacketHandler(0x23, length: 26, ingame: true)]
    public static event PacketEventHandler<DragEffectEventArgs>? OnDragUpdate;

    [PacketHandler(0xC0, length: 36, ingame: true)]
    public static event PacketEventHandler<HuedEffectEventArgs>? OnHuedUpdate;

    [PacketHandler(0xC7, length: 49, ingame: true)]
    public static event PacketEventHandler<ParticleEffectEventArgs>? OnParticleUpdate;

    [PacketHandler(0x70, length: 28, ingame: true)]
    public static event PacketEventHandler<ScreenEffectEventArgs>? OnScreenUpdate;
}
