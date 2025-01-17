using Client.Game.Context;
using Client.Game.Data;
using Client.Networking.Arguments;
using Client.Networking;
public sealed class Mobile : MobileContext
{
    #region Networking Events
    [PacketHandler(0x22, length: 11, ingame: true, extCmd: true)]
    public static event PacketEventHandler<DamageEventArgs>? OnDamage;

    [PacketHandler(0xAF, length: 13, ingame: true)]
    public static event PacketEventHandler<DeathAnimationEventArgs>? OnDeathAnimation;

    [PacketHandler(0x6E, length: 14, ingame: true)]
    public static event PacketEventHandler<MobileAnimationEventArgs>? OnAnimation;

    [PacketHandler(0x2D, length: 17, ingame: true)]
    public static event PacketEventHandler<MobileAttributesEventArgs>? OnChangedAttributes;

    [PacketHandler(0x0B, length: 7, ingame: true)]
    public static event PacketEventHandler<MobileDamageEventArgs>? OnMobileDamage;

    [PacketHandler(0xA1, length: 9, ingame: true)]
    public static event PacketEventHandler<MobileHitsEventArgs>? OnChangedHits;

    [PacketHandler(0x78, length: -1, ingame: true)]
    public static event PacketEventHandler<MobileIncomingEventArgs>? OnIncoming;

    [PacketHandler(0xA2, length: 9, ingame: true)]
    public static event PacketEventHandler<MobileManaEventArgs>? OnChangedMana;

    [PacketHandler(0x77, length: 17, ingame: true)]
    public static event PacketEventHandler<MobileMovingEventArgs>? OnMoving;

    [PacketHandler(0xA3, length: 9, ingame: true)]
    public static event PacketEventHandler<MobileStamEventArgs>? OnChangedStamina;

    [PacketHandler(0x11, length: -1, ingame: true)]
    public static event PacketEventHandler<MobileStatusEventArgs>? OnStatus;

    [PacketHandler(0x20, length: 19, ingame: true)]
    public static event PacketEventHandler<MobileUpdateEventArgs>? OnUpdate;

    [PacketHandler(0x22, length: 3, ingame: true)]
    public static event PacketEventHandler<MovementAckEventArgs>? OnMovementAck;

    [PacketHandler(0x21, length: 8, ingame: true)]
    public static event PacketEventHandler<MovementRejEventArgs>? OnMovementRej;

    [PacketHandler(0x72, length: 5, ingame: true)] // NOTE: Maybe this should be elsewhere?
    public static event PacketEventHandler<SetWarModeEventArgs>? OnWarmode;

    [PacketHandler(0x11, length: 17, ingame: true, extCmd: true)]
    public static event PacketEventHandler<StatueAnimationEventArgs>? OnStatueAnimation;
    #endregion

    public Mobile(Serial serial) : base(serial)
    {
    }

    static Mobile()
    {
    }
}
