namespace Client.Networking.Incoming;
public enum ProtocolExtensionType
{
    Accept = 0,
    PartyTrack = 1,
    GuildTrack = 2,
    Runebooks = 3,
    Guardline = 4,
}
public sealed class ProtocolExtensionEventArgs : EventArgs
{
    public NetState State { get; }
    public ProtocolExtensionEventArgs(NetState state) => State = state;
    public ProtocolExtensionType Type { get; set; }
    public PartyMemberInfo[]? Party { get; set; }
    public GuildMemberInfo[]? Guild { get; set; }
}
public sealed class GuildMemberInfo
{
    public int Serial { get; }
    public short X { get; }
    public short Y { get; }
    public byte MapID { get; }
    public byte Health { get; }
    private GuildMemberInfo(PacketReader pvSrc)
    {
        Serial = pvSrc.ReadInt32();
        X = pvSrc.ReadInt16();
        Y = pvSrc.ReadInt16();
        MapID = pvSrc.ReadByte();
        Health = pvSrc.ReadByte();
    }
    public static GuildMemberInfo Instantiate(PacketReader pvSrc) => new GuildMemberInfo(pvSrc);
}
public sealed class PartyMemberInfo
{
    public int Serial { get; }
    public short X { get; }
    public short Y { get; }
    public byte MapID { get; }
    private PartyMemberInfo(PacketReader pvSrc)
    {
        Serial = pvSrc.ReadInt32();
        X = pvSrc.ReadInt16();
        Y = pvSrc.ReadInt16();
        MapID = pvSrc.ReadByte();
    }
    public static PartyMemberInfo Instantiate(PacketReader pvSrc) => new PartyMemberInfo(pvSrc);
}
public partial class Shard
{
    public static event PacketEventHandler<ProtocolExtensionEventArgs>? OnProtocolExtension;

    [PacketHandler(0xF0, length: -1, ingame: true)]
    protected static void Received_ProtocolExtension(NetState ns, PacketReader pvSrc)
    {
        ProtocolExtensionEventArgs e = new(ns);
        ProtocolExtensionType type = (ProtocolExtensionType)pvSrc.ReadByte();
        e.Type = type;
        switch (type)
        {
            case ProtocolExtensionType.Accept:
                pvSrc.ReadByte();
                break;

            case ProtocolExtensionType.PartyTrack:
                List<PartyMemberInfo> party = new();
                while (pvSrc.ReadInt32() != 0x00)
                {
                    pvSrc.Seek(-4, SeekOrigin.Current);
                    party.Add(PartyMemberInfo.Instantiate(pvSrc));
                }
                e.Party = party.ToArray();
                break;

            case ProtocolExtensionType.GuildTrack:
                List<GuildMemberInfo> guild = new();
                while (pvSrc.ReadInt32() != 0x00)
                {
                    pvSrc.Seek(-4, SeekOrigin.Current);
                    guild.Add(GuildMemberInfo.Instantiate(pvSrc));
                }
                e.Guild = guild.ToArray();
                break;

            default:
                pvSrc.Trace();
                return;
        }
        OnProtocolExtension?.Invoke(e);
    }
}