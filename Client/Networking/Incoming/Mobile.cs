namespace Client.Networking.Incoming;

public partial class Mobile
{
    public static event PacketEventHandler<DamageEventArgs>? OnDamage;
    public static event PacketEventHandler<DeathAnimationEventArgs>? OnDeathAnimation;
    public static event PacketEventHandler<MobileAnimationEventArgs>? OnAnimation;
    public static event PacketEventHandler<MobileAttributesEventArgs>? OnChangedAttributes;
    public static event PacketEventHandler<MobileDamageEventArgs>? OnMobileDamage;
    public static event PacketEventHandler<MobileHitsEventArgs>? OnChangedHits;
    public static event PacketEventHandler<MobileIncomingEventArgs>? OnIncoming;
    public static event PacketEventHandler<MobileManaEventArgs>? OnChangedMana;
    public static event PacketEventHandler<MobileMovingEventArgs>? OnMoving;
    public static event PacketEventHandler<MobileStamEventArgs>? OnChangedStamina;
    public static event PacketEventHandler<MobileStatusEventArgs>? OnStatus;
    public static event PacketEventHandler<MobileUpdateEventArgs>? OnUpdate;
    public static event PacketEventHandler<MovementAckEventArgs>? OnMovementAck;
    public static event PacketEventHandler<MovementRejEventArgs>? OnMovementRej;
    public static event PacketEventHandler<SetWarModeEventArgs>? OnWarmode;
    public static event PacketEventHandler<StatueAnimationEventArgs>? OnStatueAnimation;

    [PacketHandler(0x11, length: 17, ingame: true, extCmd: true)]
    protected static void Received_StatueAnimation(NetState ns, PacketReader ip) => OnStatueAnimation?.Invoke(new(ns, ip));

    [PacketHandler(0x72, length: 5, ingame: true)] // NOTE: Maybe this should be elsewhere?
    protected static void Receive_SetWarmode(NetState ns, PacketReader ip) => OnWarmode?.Invoke(new(ns, ip));

    [PacketHandler(0x21, length: 8, ingame: true)]
    protected static void Received_MovementRej(NetState ns, PacketReader ip) => OnMovementRej?.Invoke(new(ns, ip));

    [PacketHandler(0x22, length: 3, ingame: true)]
    protected static void Received_MovementAck(NetState ns, PacketReader ip) => OnMovementAck?.Invoke(new(ns, ip));

    [PacketHandler(0x20, length: 19, ingame: true)]
    protected static void Received_MobileUpdate(NetState ns, PacketReader ip) => OnUpdate?.Invoke(new(ns, ip));

    [PacketHandler(0x11, length: -1, ingame: true)]
    protected static void Received_MobileStatus(NetState ns, PacketReader ip) => OnStatus?.Invoke(new(ns, ip));

    [PacketHandler(0xA3, length: 9, ingame: true)]
    protected static void Received_MobileStam(NetState ns, PacketReader ip) => OnChangedStamina?.Invoke(new(ns, ip));

    [PacketHandler(0x77, length: 17, ingame: true)]
    protected static void Received_MobileMoving(NetState ns, PacketReader ip) => OnMoving?.Invoke(new(ns, ip));

    [PacketHandler(0xA2, length: 9, ingame: true)]
    protected static void Received_MobileMana(NetState ns, PacketReader ip) => OnChangedMana?.Invoke(new(ns, ip));

    [PacketHandler(0x78, length: -1, ingame: true)]
    protected static void Received_MobileIncoming(NetState ns, PacketReader ip)
    {
        MobileIncomingEventArgs e = new(ns, ip);
        World.Received_WorldIncomingItem(ns, ip);
        OnIncoming?.Invoke(e);
    }

    [PacketHandler(0xA1, length: 9, ingame: true)]
    protected static void Received_MobileHits(NetState ns, PacketReader ip) => OnChangedHits?.Invoke(new(ns, ip));

    [PacketHandler(0x0B, length: 7, ingame: true)]
    protected static void Received_MobileDamage(NetState ns, PacketReader ip) => OnMobileDamage?.Invoke(new(ns, ip));

    [PacketHandler(0x2D, length: 17, ingame: true)]
    protected static void Received_Attributes(NetState ns, PacketReader ip) => OnChangedAttributes?.Invoke(new(ns, ip));

    [PacketHandler(0x6E, length: 14, ingame: true)]
    protected static void Received_Animation(NetState ns, PacketReader ip) => OnAnimation?.Invoke(new(ns, ip));

    [PacketHandler(0xAF, length: 13, ingame: true)]
    protected static void Received_DeathAnimation(NetState ns, PacketReader ip) => OnDeathAnimation?.Invoke(new(ns, ip));

    [PacketHandler(0x22, length: 11, ingame: true, extCmd: true)]
    protected static void Receive_Damage(NetState ns, PacketReader ip) => OnDamage?.Invoke(new(ns, ip));
}