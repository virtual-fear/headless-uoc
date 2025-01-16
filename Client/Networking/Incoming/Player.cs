namespace Client.Networking.Incoming;
public partial class Player
{
    [Obsolete] public static event PacketEventHandler<BondedStatusEventArgs>? Player_BondedStatus;
    public static event PacketEventHandler<CancelArrowEventArgs>? OnCancelArrow;
    public static event PacketEventHandler<ChangeCombatantEventArgs>? OnChangeCombatant;
    public static event PacketEventHandler<ClearWeaponAbilityEventArgs>? OnClearWeaponAbility;
    public static event PacketEventHandler<DeathStatusEventArgs>? OnDeathStatus;
    public static event PacketEventHandler<EquipUpdateEventArgs>? OnEquipmentUpdate;
    public static event PacketEventHandler<FightingEventArgs>? OnFighting;
    public static event PacketEventHandler<HealthbarEventArgs>? UpdateHealthbar;
    public static event PacketEventHandler<LiftRejEventArgs>? OnLiftRej;
    public static event PacketEventHandler<MultiTargetEventArgs>? OnMultiTarget;
    public static event PacketEventHandler<PingReqEventArgs>? OnPingAck;
    public static event PacketEventHandler<SecureTradeEventArgs>? OnSecureTrade;
    public static event PacketEventHandler<ServerChangeEventArgs>? OnServerChange;
    public static event PacketEventHandler<SkillUpdateEventArgs>? OnSkillUpdate;
    public static event PacketEventHandler<SpellbookContentEventArgs>? UpdateSpellbookContent; // (ext) packetID: 0x1B
    public static event PacketEventHandler<StatLockInfoEventArgs>? UpdateStatLockInfo; // (ext) packetID: 0x19 (TODO-Fix: Same as bonded status?
    public static event PacketEventHandler<TargetReqEventArgs>? OnTargetRequest;
    public static event PacketEventHandler<ToggleSpecialAbilityEventArgs>? OnToggleSpecialAbility;

    [PacketHandler(0x25, length: 7, ingame: true, extCmd: true)]
    protected static void Receive_ToggleSpecialAbility(NetState ns, PacketReader ip) => OnToggleSpecialAbility?.Invoke(new ToggleSpecialAbilityEventArgs(ns, ip));

    [PacketHandler(0x6C, length: 19, ingame: true)]
    protected static void Receive_TargetRequest(NetState ns, PacketReader ip) => OnTargetRequest?.Invoke(new(ns, ip));

    [PacketHandler(0x19, length: 12, ingame: true, extCmd: true)]
    protected static void Receive_StatLockInfo(NetState ns, PacketReader ip) => UpdateStatLockInfo?.Invoke(new(ns, ip));

    [PacketHandler(0x1B, length: 23, ingame: true, extCmd: true)]
    protected static void Receive_SpellbookContent(NetState ns, PacketReader ip) => UpdateSpellbookContent?.Invoke(new(ns, ip));
 
    [PacketHandler(0x3A, length: -1, ingame: true)]
    protected static void Receive_SkillUpdate(NetState ns, PacketReader ip) => OnSkillUpdate?.Invoke(new(ns, ip));

    [PacketHandler(0x76, length: 16, ingame: true)]
    protected static void Receive_ServerChange(NetState ns, PacketReader ip) => OnServerChange?.Invoke(new(ns, ip));

    [PacketHandler(0x6F, length: -1, ingame: true)]
    protected static void Receive_SecureTrade(NetState ns, PacketReader ip) => OnSecureTrade?.Invoke(new(ns, ip));

    [PacketHandler(0x73, length: 2, ingame: true)]
    protected static void Receive_PingAck(NetState ns, PacketReader pvSrc) => OnPingAck?.Invoke(new(ns, pvSrc));
 
    [PacketHandler(0x99, length: 26, ingame: true)]
    protected static void Receive_MultiTarget(NetState ns, PacketReader ip) => OnMultiTarget?.Invoke(new(ns, ip));

    [PacketHandler(0x27, length: 2, ingame: true)]
    protected static void Receive_LiftRej(NetState ns, PacketReader ip) => OnLiftRej?.Invoke(new(ns, ip));

    [PacketHandler(0x17, length: 12, ingame: true)]
    protected static void Received_Healthbar(NetState ns, PacketReader ip) => UpdateHealthbar?.Invoke(new(ns, ip));

    [PacketHandler(0x2F, length: 10, ingame: true)] // RunUO: Swing
    protected static void Receive_Fighting(NetState ns, PacketReader ip) => OnFighting?.Invoke(new(ns, ip));

    [PacketHandler(0x2E, length: 15, ingame: true)]
    protected static void Receive_EquipmentUpdate(NetState ns, PacketReader ip) => OnEquipmentUpdate?.Invoke(new(ns, ip));
    
    [PacketHandler(0x2C, length: 2, ingame: true)]
    protected static void Receive_DeathStatus(NetState ns, PacketReader ip) => OnDeathStatus?.Invoke(new(ns, ip));

    [PacketHandler(0x21, length: 5, ingame: true, extCmd: true)]
    protected static void Receive_ClearWeaponAbility(NetState ns, PacketReader ip) => OnClearWeaponAbility?.Invoke(new(ns));

    [PacketHandler(0xAA, length: 5, ingame: true)]
    protected static void Receive_ChangeCombatant(NetState ns, PacketReader ip) => OnChangeCombatant?.Invoke(new(ns, ip));

    [PacketHandler(0xBA, length: 6, ingame: true)]
    protected static void Receive_CancelArrow(NetState ns, PacketReader ip) => OnCancelArrow?.Invoke(new CancelArrowEventArgs(ns, ip));

    [Obsolete("StatLockInfo is the same")]
    //[PacketHandler(0x19, length: 11, ingame: true, extCmd: true)]
    protected static void Receive_BondedStatus(NetState ns, PacketReader ip) => Player_BondedStatus?.Invoke(new(ns, ip));
}
