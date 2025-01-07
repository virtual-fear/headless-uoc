namespace Client.Game.Data.Other;
using Client.Networking;
public sealed class CityInfo
{
    public byte Index { get; }
    public string City { get; }
    public string Building { get; }
    public int X { get; }
    public int Y { get; }
    public int Z { get; }
    public int MapID { get; }
    public int Description { get; }
    private CityInfo(PacketReader pvSrc)
    {
        Index = pvSrc.ReadByte();
        City = pvSrc.ReadString(32);
        Building = pvSrc.ReadString(32);
        X = pvSrc.ReadInt32();
        Y = pvSrc.ReadInt32();
        Z = pvSrc.ReadInt32();
        MapID = pvSrc.ReadInt32();
        Description = pvSrc.ReadInt32();
        pvSrc.ReadInt32();  //  0
    }
    public static CityInfo[] Instantiate(PacketReader pvSrc)
    {
        int count = pvSrc.ReadByte();
        if (count < 2)
            return new CityInfo[0]; //No cities

        CityInfo[] info = new CityInfo[count - 2];
        for (int i = 0; i < info.Length; i++)
            info[i] = new CityInfo(pvSrc);

        return info;
    }
    public override string ToString() => $"({X}, {Y}, {Z}) {City}, {Building}";
}