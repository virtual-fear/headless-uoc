namespace Client.Networking.Incoming;

using Client.Game.Context;
using Client.Game.Data;
using static PacketSink;
public partial class PacketSink
{
    #region EventArgs

    public sealed class MobileUpdateEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileUpdateEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short Body { get; set; }
        public short Hue { get; set; }
        public byte PacketFlags { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public Direction Direction { get; set; }
        public sbyte Z { get; set; }
    }
    public class MobileStatusEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileStatusEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public string Name { get; set; }
        public short Hits { get; set; }
        public short MaxHits { get; set; }
        public bool IsPet { get; set; }
        public byte Type { get; set; }
        public byte Gender { get; set; }
        public short Str { get; set; }
        public short Dex { get; set; }
        public short Int { get; set; }
        public short Stam { get; set; }
        public short MaxStam { get; set; }
        public short Mana { get; set; }
        public short MaxMana { get; set; }
        public int TotalGold { get; set; }
        public short Armor { get; set; }
        public short Weight { get; set; }
        public short MaxWeight { get; set; }
        public byte RaceID { get; set; }
        public short StatCap { get; set; }
        public byte Followers { get; set; }
        public byte MaxFollowers { get; set; }
        public short FireResistance { get; set; }
        public short ColdResistance { get; set; }
        public short PoisonResistance { get; set; }
        public short EnergyResistance { get; set; }
        public short Luck { get; set; }
        public short MinimumWeaponDamage { get; set; }
        public short MaximumWeaponDamage { get; set; }
        public int TithingPoints { get; set; }
    }
    public sealed class MobileIncomingEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileIncomingEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short Body { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public sbyte Z { get; set; }
        public Direction Direction { get; set; }
        public short Hue { get; set; }
        public byte PacketFlags { get; set; }
        public Notoriety Notoriety { get; set; }
    }
    public sealed class MobileHitsEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileHitsEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short HitsMax { get; set; }
        public short Hits { get; set; }
    }
    public sealed class MobileManaEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileManaEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short ManaMax { get; set; }
        public short Mana { get; set; }
    }
    public sealed class MobileStamEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileStamEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short StamMax { get; set; }
        public short Stam { get; set; }
    }
    public sealed class MobileAttributesEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileAttributesEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public short MaxHits { get; set; }
        public short Hits { get; set; }
        public short MaxMana { get; set; }
        public short Mana { get; set; }
        public short MaxStam { get; set; }
        public short Stam { get; set; }
    }
    public sealed class MobileAnimationEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileAnimationEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public int Action { get; set; }
        public int FrameCount { get; set; }
        public int RepeatCount { get; set; }
        public bool Forward { get; set; }
        public bool Repeat { get; set; }
        public byte Delay { get; set; }
    }
    public sealed class DeathAnimationEventArgs : EventArgs
    {
        public NetState State { get; }
        public DeathAnimationEventArgs(NetState state) => State = state;
        public MobileContext Mobile { get; set; }
        public Item Corpse { get; set; }
    }
    public sealed class MobileDamageEventArgs : EventArgs
    {
        public NetState State { get; }
        public MobileDamageEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public ushort Amount { get; set; }
    }
    public sealed class SetWarModeEventArgs : EventArgs
    {
        public NetState State { get; }
        public SetWarModeEventArgs(NetState state) => State = state;
        public bool Enabled { get; set; }
    }
    public sealed class DamageEventArgs : EventArgs
    {
        public NetState State { get; }
        public DamageEventArgs(NetState state) => State = state;
        public int Mobile { get; set; }
        public int Amount { get; set; }
    }

    #endregion (done)

    public static event PacketEventHandler<MobileUpdateEventArgs>? MobileUpdate;
    public static event PacketEventHandler<MobileStatusEventArgs>? MobileStatus;
    public static event PacketEventHandler<MobileIncomingEventArgs>? MobileIncoming;
    public static event PacketEventHandler<MobileHitsEventArgs>? MobileHits;
    public static event PacketEventHandler<MobileManaEventArgs>? MobileMana;
    public static event PacketEventHandler<MobileStamEventArgs>? MobileStam;
    public static event PacketEventHandler<MobileAttributesEventArgs>? MobileAttributes;
    public static event PacketEventHandler<MobileAnimationEventArgs>? MobileAnimation;
    public static event PacketEventHandler<DeathAnimationEventArgs>? DeathAnimation;
    public static event PacketEventHandler<MobileDamageEventArgs>? MobileDamage;
    public static event PacketEventHandler<SetWarModeEventArgs>? SetWarMode;
    public static event PacketEventHandler<DamageEventArgs>? Damage;
    public static void InvokeDamage(DamageEventArgs e) => Damage?.Invoke(e);
    public static void InvokeSetWarMode(SetWarModeEventArgs e) => SetWarMode?.Invoke(e);
    public static void InvokeMobileDamage(MobileDamageEventArgs e) => MobileDamage?.Invoke(e);
    public static void InvokeDeathAnimation(DeathAnimationEventArgs e) => DeathAnimation?.Invoke(e);
    public static void InvokeMobileAnimation(MobileAnimationEventArgs e) => MobileAnimation?.Invoke(e);
    public static void InvokeMobileAttributes(MobileAttributesEventArgs e) => MobileAttributes?.Invoke(e);
    public static void InvokeMobileStam(MobileStamEventArgs e) => MobileStam?.Invoke(e);
    public static void InvokeMobileMana(MobileManaEventArgs e) => MobileMana?.Invoke(e);
    public static void InvokeMobileHits(MobileHitsEventArgs e) => MobileHits?.Invoke(e);
    public static void InvokeMobileIncoming(MobileIncomingEventArgs e) => MobileIncoming?.Invoke(e);
    public static void InvokeMobileStatus(MobileStatusEventArgs e) => MobileStatus?.Invoke(e);
    public static void InvokeMobileUpdate(MobileUpdateEventArgs e) => MobileUpdate?.Invoke(e);

}
public static class UpdatedMobile
{
    public static void Configure()
    {
        Register(0x20, 19, true, new OnPacketReceive(MobileUpdate));
        Register(0x11, -1, true, new OnPacketReceive(MobileStatus));
        Register(0x78, -1, true, new OnPacketReceive(MobileIncoming));
        Register(0xA1, 09, true, new OnPacketReceive(MobileHits));
        Register(0xA2, 09, true, new OnPacketReceive(MobileMana));
        Register(0xA3, 09, true, new OnPacketReceive(MobileStam));
        Register(0x2D, 17, true, new OnPacketReceive(MobileAttributes));
        Register(0x6E, 14, true, new OnPacketReceive(MobileAnimation));
        Register(0xAF, 13, true, new OnPacketReceive(DeathAnimation));
        Register(0x0B, 07, true, new OnPacketReceive(MobileDamage));
        Register(0x72, 05, true, new OnPacketReceive(SetWarMode)); // NOTE: Maybe this should be elsewhere?
        RegisterExtended(0x22, 11, true, new OnPacketReceive(Damage));

        MobileContext.Configure();
    }

    private static void Damage(NetState ns, PacketReader pvSrc)
    {
        DamageEventArgs e = new DamageEventArgs(ns);

        pvSrc.ReadByte();

        e.Mobile = pvSrc.ReadInt32();
        e.Amount = pvSrc.ReadByte();

        PacketSink.InvokeDamage(e);
    }

    private static void SetWarMode(NetState ns, PacketReader pvSrc)
    {
        SetWarModeEventArgs e = new SetWarModeEventArgs(ns);

        e.Enabled = pvSrc.ReadBoolean();

        pvSrc.ReadByte();   //  0x00
        pvSrc.ReadByte();   //  0x32
        pvSrc.ReadByte();   //  0x00

        PacketSink.InvokeSetWarMode(e);
    }

    private static void MobileDamage(NetState ns, PacketReader pvSrc)
    {
        MobileDamageEventArgs e = new MobileDamageEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();
        e.Amount = pvSrc.ReadUInt16();

        PacketSink.InvokeMobileDamage(e);
    }

    private static void DeathAnimation(NetState ns, PacketReader pvSrc)
    {
        DeathAnimationEventArgs e = new DeathAnimationEventArgs(ns);

        e.Mobile = MobileContext.Acquire(pvSrc.ReadInt32());
        e.Corpse = Item.Acquire(pvSrc.ReadInt32());

        pvSrc.ReadInt32();

        PacketSink.InvokeDeathAnimation(e);
    }

    private static void MobileAnimation(NetState ns, PacketReader pvSrc)
    {
        MobileAnimationEventArgs e = new MobileAnimationEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();
        e.Action = pvSrc.ReadInt16();
        e.FrameCount = pvSrc.ReadInt16();
        e.RepeatCount = pvSrc.ReadInt16();
        e.Forward = pvSrc.ReadBoolean();
        e.Repeat = pvSrc.ReadBoolean();
        e.Delay = pvSrc.ReadByte();

        PacketSink.InvokeMobileAnimation(e);
    }

    private static void MobileAttributes(NetState ns, PacketReader pvSrc)
    {
        MobileAttributesEventArgs e = new MobileAttributesEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();

        e.MaxHits = pvSrc.ReadInt16();
        e.Hits = pvSrc.ReadInt16();

        e.MaxMana = pvSrc.ReadInt16();
        e.Mana = pvSrc.ReadInt16();

        e.MaxStam = pvSrc.ReadInt16();
        e.Stam = pvSrc.ReadInt16();

        PacketSink.InvokeMobileAttributes(e);
    }

    private static void MobileStam(NetState ns, PacketReader pvSrc)
    {
        MobileStamEventArgs e = new MobileStamEventArgs(ns);

        e.Serial = pvSrc.ReadInt16();
        e.StamMax = pvSrc.ReadInt16();
        e.Stam = pvSrc.ReadInt16();

        PacketSink.InvokeMobileStam(e);
    }

    private static void MobileMana(NetState ns, PacketReader pvSrc)
    {
        MobileManaEventArgs e = new MobileManaEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();
        e.ManaMax = pvSrc.ReadInt16();
        e.Mana = pvSrc.ReadInt16();

        PacketSink.InvokeMobileMana(e);
    }

    private static void MobileHits(NetState ns, PacketReader pvSrc)
    {
        MobileHitsEventArgs e = new MobileHitsEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();
        e.HitsMax = pvSrc.ReadInt16();
        e.Hits = pvSrc.ReadInt16();

        PacketSink.InvokeMobileHits(e);
    }

    private static void MobileIncoming(NetState ns, PacketReader pvSrc)
    {
        MobileIncomingEventArgs e = new MobileIncomingEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();
        e.Body = pvSrc.ReadInt16();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Z = pvSrc.ReadSByte();
        e.Direction = (Direction)pvSrc.ReadByte();
        e.Hue = pvSrc.ReadInt16();
        e.PacketFlags = pvSrc.ReadByte();
        e.Notoriety = (Notoriety)pvSrc.ReadByte();
        //m.SetLocation( m.Parent, x, y, z );
        //Mobile m = ns.Mobile;
        //if (m.Player)
        //{
        //    m.Direction = (byte)(m.Direction & 7);
        //    m.Direction = (byte)(m.Direction | (m.Direction & 128));
        //}

        ItemIncoming(ns, pvSrc);

        PacketSink.InvokeMobileIncoming(e);
    }

    public static void ItemIncoming(NetState ns, PacketReader pvSrc)
    {
        ItemIncomingEventArgs e = new ItemIncomingEventArgs(ns);
        e.Serial = pvSrc.ReadInt32();
        if (e.Serial != 0x00)
        {
            e.ItemID = pvSrc.ReadUInt16();
            e.Layer = (Layer)pvSrc.ReadByte();
            e.Hue = pvSrc.ReadInt16();
        }
        PacketSink.InvokeItemIncoming(e);
    }


    private static void MobileStatus(NetState ns, PacketReader pvSrc)
    {
        MobileStatusEventArgs e = new MobileStatusEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();
        e.Name = pvSrc.ReadString(30);
        e.Hits = pvSrc.ReadInt16();
        e.MaxHits = pvSrc.ReadInt16();
        e.IsPet = pvSrc.ReadBoolean();

        byte type = pvSrc.ReadByte();

        if (type > 0)
        {
            e.Gender = pvSrc.ReadByte();

            // 0: Male
            // 1: Female

            e.Str = pvSrc.ReadInt16();
            e.Dex = pvSrc.ReadInt16();
            e.Int = pvSrc.ReadInt16();

            e.Stam = pvSrc.ReadInt16();
            e.MaxStam = pvSrc.ReadInt16();
            e.Mana = pvSrc.ReadInt16();
            e.MaxMana = pvSrc.ReadInt16();

            e.TotalGold = pvSrc.ReadInt32();
            e.Armor = pvSrc.ReadInt16();
            e.Weight = pvSrc.ReadInt16();

            if (type >= 5)
            {
                pvSrc.ReadInt16();  //  MaxWeight
                pvSrc.ReadByte();   //  RaceID
            }

            e.StatCap = pvSrc.ReadInt16();
            e.Followers = pvSrc.ReadByte();
            e.MaxFollowers = pvSrc.ReadByte();

            if (type >= 4)
            {
                e.FireResistance = pvSrc.ReadInt16();
                e.ColdResistance = pvSrc.ReadInt16();
                e.PoisonResistance = pvSrc.ReadInt16();
                e.EnergyResistance = pvSrc.ReadInt16();
                e.Luck = pvSrc.ReadInt16();

                e.MinimumWeaponDamage = pvSrc.ReadInt16();
                e.MaximumWeaponDamage = pvSrc.ReadInt16();

                e.TithingPoints = pvSrc.ReadInt32();

                if (type >= 6)
                {
                    for (int i = 0; i < 15; ++i)
                        pvSrc.ReadInt16();  //  GetAOSStatus
                }
            }
        }

        PacketSink.InvokeMobileStatus(e);
    }

    private static void MobileUpdate(NetState ns, PacketReader pvSrc)
    {
        MobileUpdateEventArgs e = new MobileUpdateEventArgs(ns);

        e.Serial = pvSrc.ReadInt32();
        e.Body = (short)pvSrc.ReadUInt16();

        pvSrc.ReadByte();   //  0

        e.Hue = pvSrc.ReadInt16();
        e.PacketFlags = pvSrc.ReadByte();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();

        pvSrc.ReadInt16();  //  0

        e.Direction = (Direction)pvSrc.ReadByte();
        e.Z = pvSrc.ReadSByte();

        //ns.Send(Warmode.Instantiate(false));

        PacketSink.InvokeMobileUpdate(e);
    }

    static void RegisterExtended(int packetID, int length, bool variable, OnPacketReceive onReceive) => PacketHandlers.RegisterExtended(packetID, length, variable, onReceive);
    
    static void Register(int packetID, int length, bool variable, OnPacketReceive onReceive) => PacketHandlers.Register(packetID, length, variable, onReceive);
}
