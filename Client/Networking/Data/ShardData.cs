namespace Client.Networking.Data;
using System.Net;
using Client.Networking;
using Client.Networking.Arguments;
using Client.Networking.Outgoing;
public struct ShardData
{
    public byte Index { get; }
    public string Name { get; }
    public byte FullPercent { get; }
    public sbyte TimeZone { get; }
    public IPAddress Address { get; }
    private ShardData(PacketReader pvSrc)
    {
        Index = (byte)pvSrc.ReadUInt16();
        Name = pvSrc.ReadString(32);
        FullPercent = pvSrc.ReadByte();
        TimeZone = pvSrc.ReadSByte();
        int addr = pvSrc.ReadInt32();
        Address = new IPAddress(addr);
    }
    public static IEnumerable<ShardData> Instantiate(PacketReader pvSrc)
    {
        pvSrc.ReadByte();   //  Unknown
        var info = new ShardData[pvSrc.ReadUInt16()];
        for (int i = 0; i < info.Length; i++)
            info[i] = new ShardData(pvSrc);

        return info;
    }
    public void Submit(AccountLoginEventArgs e) => e.State?.Send(PPlayServer.Instantiate(Index));
}