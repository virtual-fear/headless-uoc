namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class MobileStatusEventArgs : EventArgs
{
    [PacketHandler(0x11, length: -1, ingame: true)]
    private static event PacketEventHandler<MobileStatusEventArgs>? Update;
    public NetState State { get; }
    public Serial Serial { get; }
    public string? Name { get; }
    public short Hits { get; }
    public short MaxHits { get; }
    public bool IsPet { get; }
    public byte Type { get; }
    public byte Gender { get; }
    public short Str { get; }
    public short Dex { get; }
    public short Int { get; }
    public short Stam { get; }
    public short MaxStam { get; }
    public short Mana { get; }
    public short MaxMana { get; }
    public int TotalGold { get; }
    public short Armor { get; }
    public short Weight { get; }
    public short MaxWeight { get; }
    public byte RaceID { get; }
    public short StatCap { get; }
    public byte Followers { get; }
    public byte MaxFollowers { get; }
    public short FireResistance { get; }
    public short ColdResistance { get; }
    public short PoisonResistance { get; }
    public short EnergyResistance { get; }
    public short Luck { get; }
    public short MinimumWeaponDamage { get; }
    public short MaximumWeaponDamage { get; }
    public int TithingPoints { get; }
    private MobileStatusEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        Name = ip.ReadString(30);
        Hits = ip.ReadInt16();
        MaxHits = ip.ReadInt16();
        IsPet = ip.ReadBoolean();
        byte type = ip.ReadByte();
        if (type > 0)
        {
            Gender = ip.ReadByte(); // (0x0:male, 0x1:female)
            Str = ip.ReadInt16();
            Dex = ip.ReadInt16();
            Int = ip.ReadInt16();
            Stam = ip.ReadInt16();
            MaxStam = ip.ReadInt16();
            Mana = ip.ReadInt16();
            MaxMana = ip.ReadInt16();
            TotalGold = ip.ReadInt32();
            Armor = ip.ReadInt16();
            Weight = ip.ReadInt16();
            if (type >= 5)
            {
                ip.ReadInt16();  //  MaxWeight
                ip.ReadByte();   //  RaceID
            }
            StatCap = ip.ReadInt16();
            Followers = ip.ReadByte();
            MaxFollowers = ip.ReadByte();
            if (type >= 4)
            {
                FireResistance = ip.ReadInt16();
                ColdResistance = ip.ReadInt16();
                PoisonResistance = ip.ReadInt16();
                EnergyResistance = ip.ReadInt16();
                Luck = ip.ReadInt16();
                MinimumWeaponDamage = ip.ReadInt16();
                MaximumWeaponDamage = ip.ReadInt16();
                TithingPoints = ip.ReadInt32();
                if (type >= 6)
                {
                    for (int i = 0; i < 15; ++i)
                        ip.ReadInt16();  //  GetAOSStatus
                }
            }
        }
    }

    static MobileStatusEventArgs() => Update += MobileStatusEventArgs_Update;
    private static void MobileStatusEventArgs_Update(MobileStatusEventArgs e)
        => Mobile.Acquire(e.Serial).UpdateStatus(e);
}