using Client.Game;
namespace Client.Networking.Incoming;
using static PacketSink;
public partial class PacketSink
{
    #region EventArgs

    public sealed class EquipUpdateEventArgs : EventArgs
    {
        public NetState State { get; }
        public EquipUpdateEventArgs(NetState state) => State = state;
        public int Item { get; set; }
        public int ID { get; set; }
        public Layer Layer { get; set; }
        public int Mobile { get; set; }
        public short Hue { get; set; }
    }
    public sealed class ChangeCombatantEventArgs : EventArgs
    {
        public NetState State { get; }
        public ChangeCombatantEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
    }
    public sealed class LiftRejEventArgs : EventArgs
    {
        public NetState State { get; }
        public LiftRejEventArgs(NetState state) => State = state;
        public LRReason Reason { get; set; }
    }
    public sealed class DeathStatusEventArgs : EventArgs
    {
        public NetState State { get; }
        public DeathStatusEventArgs(NetState state) => State = state;
        public bool Dead { get; set; }
    }
    public sealed class FightingEventArgs : EventArgs
    {
        public NetState State { get; }
        public FightingEventArgs(NetState state) => State = state;
        public byte Flag { get; set; }
        public int Attacker { get; set; }
        public int Defender { get; set; }
    }
    public sealed class SkillUpdateEventArgs : EventArgs
    {
        public NetState State { get; }
        public SkillUpdateEventArgs(NetState state) => State = state;
        public byte Type { get; set; }
        public List<SkillInfo> Skills { get; set; }
    }
    public sealed class SecureTradeEventArgs : EventArgs
    {
        public NetState State { get; }
        public SecureTradeEventArgs(NetState state) => State = state;
        public int Them { get; set; }
        public int FirstContainer { get; set; }
        public int SecondContainer { get; set; }
        public string Name { get; set; }
    }
    public sealed class CancelArrowEventArgs : EventArgs
    {
        public NetState State { get; }
        public CancelArrowEventArgs(NetState state) => State = state;
    }
    public sealed class ServerChangeEventArgs : EventArgs
    {
        public NetState State { get; }
        public ServerChangeEventArgs(NetState state) => State = state;
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        public short XMap { get; set; }
        public short YMap { get; set; }
    }
    public sealed class MultiTargetEventArgs : EventArgs
    {
        public NetState State { get; }
        public MultiTargetEventArgs(NetState state) => State = state;
        public bool AllowGround { get; set; }
        public int TargetID { get; set; }
        public TargetFlags Flags { get; set; }
        public short MultiID { get; set; }
        public short OffsetX { get; set; }
        public short OffsetY { get; set; }
        public short OffsetZ { get; set; }
    }
    public sealed class HealthbarEventArgs : EventArgs
    {
        public NetState State { get; }
        public HealthbarEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public HealthbarType Type { get; set; }
        public byte Level { get; set; }
    }
    public sealed class StatLockInfoEventArgs : EventArgs
    {
        public NetState State { get; }
        public StatLockInfoEventArgs(NetState state) => State = state;
        public int Mobile { get; set; }

    }
    public sealed class BondedStatusEventArgs : EventArgs
    {
        public NetState State { get; }
        public BondedStatusEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public byte Value01 { get; set; }
        public byte Value02 { get; set; }
    }
    public sealed class SpellbookContentEventArgs : EventArgs
    {
        public NetState State { get; }
        public SpellbookContentEventArgs(NetState state) => State = state;
        public int Item { get; set; }
        public short Graphic { get; set; }
        public short Offset { get; set; }
        public long Content { get; set; }
    }
    public sealed class ClearWeaponAbilityEventArgs : EventArgs
    {
        public NetState State { get; }
        public ClearWeaponAbilityEventArgs(NetState state) => State = state;
    }
    public sealed class ToggleSpecialAbilityEventArgs : EventArgs
    {
        public NetState State { get; }
        public ToggleSpecialAbilityEventArgs(NetState state) => State = state;
        public short AbilityID { get; set; }
        public bool Active { get; set; }
    }

    #endregion (done)

    public static event PacketEventHandler<EquipUpdateEventArgs>? EquipUpdate;
    public static event PacketEventHandler<ChangeCombatantEventArgs>? ChangeCombatant;
    public static event PacketEventHandler<LiftRejEventArgs>? LiftRej;
    public static event PacketEventHandler<DeathStatusEventArgs>? DeathStatus;
    public static event PacketEventHandler<FightingEventArgs>? Fighting;
    public static event PacketEventHandler<SkillUpdateEventArgs>? SkillUpdate;
    public static event PacketEventHandler<SecureTradeEventArgs>? SecureTrade;
    public static event PacketEventHandler<CancelArrowEventArgs>? CancelArrow;
    public static event PacketEventHandler<ServerChangeEventArgs>? ServerChange;
    public static event PacketEventHandler<MultiTargetEventArgs>? MultiTarget;
    public static event PacketEventHandler<HealthbarEventArgs>? Healthbar;
    public static event PacketEventHandler<StatLockInfoEventArgs>? StatLockInfo;                 // (ext) packetID: 0x19 (TODO-Fix: Same as bonded status?
    public static event PacketEventHandler<BondedStatusEventArgs>? BondedStatus;                 // (ext) packetID: 0x19
    public static event PacketEventHandler<SpellbookContentEventArgs>? SpellbookContent;         // (ext) packetID: 0x1B
    public static event PacketEventHandler<ClearWeaponAbilityEventArgs>? ClearWeaponAbility;     // (ext) packetID: 0x21
    public static event PacketEventHandler<ToggleSpecialAbilityEventArgs>? ToggleSpecialAbility; // (ext) packetID: 0x25
    public static void InvokeToggleSpecialAbility(ToggleSpecialAbilityEventArgs e) => ToggleSpecialAbility?.Invoke(e);
    public static void InvokeClearWeaponAbility(ClearWeaponAbilityEventArgs e) => ClearWeaponAbility?.Invoke(e);
    public static void InvokeSpellbookContent(SpellbookContentEventArgs e) => SpellbookContent?.Invoke(e);
    public static void InvokeBondedStatus(BondedStatusEventArgs e) => BondedStatus?.Invoke(e);
    public static void InvokeStatLockInfo(StatLockInfoEventArgs e) => StatLockInfo?.Invoke(e);
    public static void InvokeHealthbar(HealthbarEventArgs e) => Healthbar?.Invoke(e);
    public static void InvokeMultiTarget(MultiTargetEventArgs e) => MultiTarget?.Invoke(e);
    public static void InvokeServerChange(ServerChangeEventArgs e) => ServerChange?.Invoke(e);
    public static void InvokeCancelArrow(CancelArrowEventArgs e) => CancelArrow?.Invoke(e);
    public static void InvokeSecureTrade(SecureTradeEventArgs e) => SecureTrade?.Invoke(e);
    public static void InvokeSkillUpdate(SkillUpdateEventArgs e) => SkillUpdate?.Invoke(e);
    public static void InvokeFighting(FightingEventArgs e) => Fighting?.Invoke(e);
    public static void InvokeDeathStatus(DeathStatusEventArgs e) => DeathStatus?.Invoke(e);
    public static void InvokeLiftRej(LiftRejEventArgs e) => LiftRej?.Invoke(e);
    public static void InvokeChangeCombatant(ChangeCombatantEventArgs e) => ChangeCombatant?.Invoke(e);
    public static void InvokeEquipUpdate(EquipUpdateEventArgs e) => EquipUpdate?.Invoke(e);
}

public class SkillInfo
{
    public int SkillID { get; }
    public ushort UV { get; }
    public ushort Base { get; }
    public byte Locked { get; }
    public ushort Cap { get; }
    private SkillInfo(PacketReader pvSrc)
    {
        SkillID = pvSrc.ReadUInt16();
        UV = pvSrc.ReadUInt16();
        Base = pvSrc.ReadUInt16();
        Locked = pvSrc.ReadByte();
        Cap = pvSrc.ReadUInt16();
    }
    public static SkillInfo Instantiate(PacketReader pvSrc) => new SkillInfo(pvSrc);
}
public enum HealthbarType : byte
{
    Invalid = 0x00,
    Poison = 0x01,
    Yellow = 0x02,
}
public enum LRReason : byte
{
    CannotLift = 0,
    OutOfRange = 1,
    OutOfSight = 2,
    TryToSteal = 3,
    AreHolding = 4,
    Inspecific = 5
}
public static class UpdatedPlayer
{
    public static void Configure()
    {
        Register(0x2E, 15, true, new OnPacketReceive(EquipmentUpdate));
        Register(0xAA, 05, true, new OnPacketReceive(ChangeCombatant));
        Register(0x27, 02, true, new OnPacketReceive(LiftRej));
        Register(0x2C, 02, true, new OnPacketReceive(DeathStatus));
        Register(0x2F, 10, true, new OnPacketReceive(Fighting)); // runuo:  Swing
        Register(0x3A, -1, true, new OnPacketReceive(SkillUpdate));
        Register(0x6F, -1, true, new OnPacketReceive(SecureTrade));
        Register(0xBA, 06, true, new OnPacketReceive(CancelArrow));
        Register(0x76, 16, true, new OnPacketReceive(ServerChange));
        Register(0x99, 26, true, new OnPacketReceive(MultiTarget));
        Register(0x17, 12, true, new OnPacketReceive(Healthbar));
        RegisterExtended(0x19, 12, true, new OnPacketReceive(StatLockInfo)); // TODO: fix lockBits
        //RegisterExtended(0x19, 11, true, new OnPacketReceive(BondedStatus));
        RegisterExtended(0x1B, 23, true, new OnPacketReceive(SpellbookContent));
        RegisterExtended(0x21, 05, true, new OnPacketReceive(ClearWeaponAbility));
        RegisterExtended(0x25, 07, true, new OnPacketReceive(ToggleSpecialAbility));

    }
    private static void ToggleSpecialAbility(NetState ns, PacketReader pvSrc)
    {
        ToggleSpecialAbilityEventArgs e = new ToggleSpecialAbilityEventArgs(ns);

        e.AbilityID = pvSrc.ReadInt16();
        e.Active = pvSrc.ReadBoolean();

        PacketSink.InvokeToggleSpecialAbility(e);
    }
    private static void ClearWeaponAbility(NetState ns, PacketReader pvSrc)
    {
        ClearWeaponAbilityEventArgs e = new ClearWeaponAbilityEventArgs(ns);

        PacketSink.InvokeClearWeaponAbility(e);
    }
    private static void SpellbookContent(NetState ns, PacketReader pvSrc)
    {
        SpellbookContentEventArgs e = new SpellbookContentEventArgs(ns);

        pvSrc.ReadInt16();  //  0x01

        e.Item = pvSrc.ReadInt32();
        e.Graphic = pvSrc.ReadInt16();
        e.Offset = pvSrc.ReadInt16();

        long content = 0;

        for (int i = 0; i < 8; ++i)
            content += pvSrc.ReadByte() << (i * 8);

        // todo: fix the content

        e.Content = content;

        PacketSink.InvokeSpellbookContent(e);
    }
    private static void BondedStatus(NetState ns, PacketReader pvSrc)
    {
        BondedStatusEventArgs e = new BondedStatusEventArgs(ns);

        byte v1, v2;

        v1 = pvSrc.ReadByte();
        e.Serial = pvSrc.ReadInt32();
        v2 = pvSrc.ReadByte();

        e.Value01 = v1;
        e.Value02 = v2;

        PacketSink.InvokeBondedStatus(e);
    }
    private static void StatLockInfo(NetState ns, PacketReader pvSrc)
    {
        StatLockInfoEventArgs e = new StatLockInfoEventArgs(ns);

        pvSrc.ReadByte();

        e.Mobile = pvSrc.ReadInt32();

        pvSrc.ReadByte();

        byte lockBits = pvSrc.ReadByte();

        // todo //

        PacketSink.InvokeStatLockInfo(e);
    }

    private static void Healthbar(NetState ns, PacketReader pvSrc)
    {
        HealthbarEventArgs e = new HealthbarEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();
        pvSrc.Seek(2, SeekOrigin.Current);
        e.Type = (HealthbarType)pvSrc.ReadInt16();
        e.Level = pvSrc.ReadByte();

        PacketSink.InvokeHealthbar(e);
    }
    private static void MultiTarget(NetState ns, PacketReader pvSrc)
    {
        MultiTargetEventArgs e = new MultiTargetEventArgs(ns);

        e.AllowGround = pvSrc.ReadBoolean();
        e.TargetID = pvSrc.ReadInt32();
        e.Flags = (TargetFlags)pvSrc.ReadByte();

        pvSrc.Seek(18, SeekOrigin.Begin);

        e.MultiID = pvSrc.ReadInt16();
        e.OffsetX = pvSrc.ReadInt16();
        e.OffsetY = pvSrc.ReadInt16();
        e.OffsetZ = pvSrc.ReadInt16();

        PacketSink.InvokeMultiTarget(e);
    }

    private static void ServerChange(NetState ns, PacketReader pvSrc)
    {
        ServerChangeEventArgs e = new ServerChangeEventArgs(ns);

        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadInt16();

        pvSrc.Seek(5, SeekOrigin.Current);

        e.XMap = pvSrc.ReadInt16();
        e.YMap = pvSrc.ReadInt16();

        PacketSink.InvokeServerChange(e);
    }

    private static void CancelArrow(NetState ns, PacketReader pvSrc)
    {
        CancelArrowEventArgs e = new CancelArrowEventArgs(ns);

        pvSrc.Seek(5, SeekOrigin.Begin);

        PacketSink.InvokeCancelArrow(e);
    }

    private static void SecureTrade(NetState ns, PacketReader pvSrc)
    {
        SecureTradeEventArgs e = new SecureTradeEventArgs(ns);

        pvSrc.ReadByte();   //  0   :   runuo:display
        e.Them = pvSrc.ReadInt32();
        e.FirstContainer = pvSrc.ReadInt32();
        e.SecondContainer = pvSrc.ReadInt32();
        pvSrc.ReadBoolean();    //  always true
        e.Name = pvSrc.ReadString(30);

        PacketSink.InvokeSecureTrade(e);
    }
    private static void SkillUpdate(NetState ns, PacketReader pvSrc)
    {
        SkillUpdateEventArgs e = new SkillUpdateEventArgs(ns);

        e.Type = pvSrc.ReadByte();  //  0x02    :   type-- absolute, capped

        List<SkillInfo> skills = new List<SkillInfo>();

        while (pvSrc.ReadInt16() != 0)
        {
            pvSrc.Seek(-2, SeekOrigin.Current);
            skills.Add(SkillInfo.Instantiate(pvSrc));
        }

        e.Skills = skills;

        PacketSink.InvokeSkillUpdate(e);
    }

    private static void Fighting(NetState ns, PacketReader pvSrc)
    {
        FightingEventArgs e = new FightingEventArgs(ns);

        e.Flag = pvSrc.ReadByte();

        e.Attacker = pvSrc.ReadInt32();
        e.Defender = pvSrc.ReadInt32();

        PacketSink.InvokeFighting(e);
    }

    private static void DeathStatus(NetState ns, PacketReader pvSrc)
    {
        DeathStatusEventArgs e = new DeathStatusEventArgs(ns);

        e.Dead = (pvSrc.ReadByte() == 2);

        PacketSink.InvokeDeathStatus(e);
    }

    private static void LiftRej(NetState ns, PacketReader pvSrc)
    {
        LiftRejEventArgs e = new LiftRejEventArgs(ns);

        e.Reason = (LRReason)pvSrc.ReadByte();

        PacketSink.InvokeLiftRej(e);
    }
    private static void ChangeCombatant(NetState ns, PacketReader pvSrc)
    {
        ChangeCombatantEventArgs e = new ChangeCombatantEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();

        PacketSink.InvokeChangeCombatant(e);
    }
    private static void EquipmentUpdate(NetState ns, PacketReader pvSrc)
    {
        EquipUpdateEventArgs e = new EquipUpdateEventArgs(ns);
        e.Item = pvSrc.ReadInt32();
        e.ID = pvSrc.ReadInt16() & 0x3FFF;
        pvSrc.Seek(1, SeekOrigin.Current);
        e.Layer = (Layer)pvSrc.ReadByte();
        e.Mobile = pvSrc.ReadInt32();
        e.Hue = pvSrc.ReadInt16();
        PacketSink.InvokeEquipUpdate(e);
    }
    private static void RegisterExtended(byte packetID, int length, bool variable, OnPacketReceive onReceive)
    {
        PacketHandlers.RegisterExtended(packetID, length, variable, onReceive);
    }
    private static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive)
    {
        PacketHandlers.Register(packetID, length, variable, onReceive);
    }
}
