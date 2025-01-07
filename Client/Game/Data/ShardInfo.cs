namespace Client.Game.Data;
using System.Net;
using Client.Networking;
using static Networking.Incoming.PacketSink;
public sealed class ShardInfo
{
    private class PlayServer : Packet
    {
        public PlayServer(ushort index) : base(0xA0, 3) => base.Stream.Write((short)index);
    }
    public ushort Index { get; }
    public string Name { get; }
    public byte FullPercent { get; }
    public sbyte TimeZone { get; }
    public IPAddress Address { get; }
    private ShardInfo(PacketReader pvSrc)
    {
        Index = pvSrc.ReadUInt16();
        Name = pvSrc.ReadString(32);
        FullPercent = pvSrc.ReadByte();
        TimeZone = pvSrc.ReadSByte();
        int addr = pvSrc.ReadInt32();
        Address = new IPAddress((int)addr);
    }
    public static IEnumerable<ShardInfo> Instantiate(PacketReader pvSrc)
    {
        pvSrc.ReadByte();   //  Unknown
        ShardInfo[] info = new ShardInfo[pvSrc.ReadUInt16()];
        for (int i = 0; i < info.Length; i++)
            info[i] = new ShardInfo(pvSrc);

        return info;
    }
    public void Submit(AccountLoginEventArgs e) => e.State.Send(new PlayServer(Index));
}
