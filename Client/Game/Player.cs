namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public static class Player
{
    //[PacketHandler(0x19, length: 11, ingame: true, extCmd: true)]
    //public static event PacketEventHandler<BondedStatusEventArgs>? Player_BondedStatus;

    [PacketHandler(0xBA, length: 6, ingame: true)]
    public static event PacketEventHandler<CancelArrowEventArgs>? OnCancelArrow;

    [PacketHandler(0xAA, length: 5, ingame: true)]
    public static event PacketEventHandler<ChangeCombatantEventArgs>? OnChangeCombatant;

    [PacketHandler(0x21, length: 5, ingame: true, extCmd: true)]
    public static event PacketEventHandler<ClearWeaponAbilityEventArgs>? OnClearWeaponAbility;

    [PacketHandler(0x2C, length: 2, ingame: true)]
    public static event PacketEventHandler<DeathStatusEventArgs>? OnDeathStatus;

    [PacketHandler(0x2E, length: 15, ingame: true)]
    public static event PacketEventHandler<EquipUpdateEventArgs>? OnEquipmentUpdate;

    [PacketHandler(0x2F, length: 10, ingame: true)] // RunUO: Swing
    public static event PacketEventHandler<FightingEventArgs>? OnFighting;

    [PacketHandler(0x17, length: 12, ingame: true)]
    public static event PacketEventHandler<HealthbarEventArgs>? UpdateHealthbar;

    [PacketHandler(0x27, length: 2, ingame: true)]
    public static event PacketEventHandler<LiftRejEventArgs>? OnLiftRej;

    [PacketHandler(0x99, length: 26, ingame: true)]
    public static event PacketEventHandler<MultiTargetEventArgs>? OnMultiTarget;

    [PacketHandler(0x73, length: 2, ingame: true)]
    public static event PacketEventHandler<PingReqEventArgs>? OnPingAck;

    [PacketHandler(0x6F, length: -1, ingame: true)]
    public static event PacketEventHandler<SecureTradeEventArgs>? OnSecureTrade;

    [PacketHandler(0x76, length: 16, ingame: true)]
    public static event PacketEventHandler<ServerChangeEventArgs>? OnServerChange;

    [PacketHandler(0x3A, length: -1, ingame: true)]
    public static event PacketEventHandler<SkillUpdateEventArgs>? OnSkillUpdate;

    [PacketHandler(0x1B, length: 23, ingame: true, extCmd: true)]
    public static event PacketEventHandler<SpellbookContentEventArgs>? UpdateSpellbookContent; // (ext) packetID: 0x1B

    [PacketHandler(0x19, length: 12, ingame: true, extCmd: true)]
    public static event PacketEventHandler<StatLockInfoEventArgs>? UpdateStatLockInfo; // (ext) packetID: 0x19 (TODO-Fix: Same as bonded status?

    [PacketHandler(0x6C, length: 19, ingame: true)]
    public static event PacketEventHandler<TargetReqEventArgs>? OnTargetRequest;

    [PacketHandler(0x25, length: 7, ingame: true, extCmd: true)]
    public static event PacketEventHandler<ToggleSpecialAbilityEventArgs>? OnToggleSpecialAbility;
}