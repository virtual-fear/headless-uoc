namespace Client.Networking.Incoming.Mobiles;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MobileStatusEventArgs>? OnMobileStatus;
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
    protected static class MobileStatus
    {
        [PacketHandler(0x11, length: -1, ingame: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            MobileStatusEventArgs e = new(ns);

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
            OnMobileStatus?.Invoke(e);
        }
    }
}
