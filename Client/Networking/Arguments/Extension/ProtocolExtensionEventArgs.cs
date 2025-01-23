namespace Client.Networking.Arguments;
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
    [PacketHandler(0xF0, length: -1, ingame: true)]
    public static event PacketEventHandler<ProtocolExtensionEventArgs>? Update; // TODO: Move data types in EventArgs to Client.Game.Data
    public NetState State { get; }
    public ProtocolExtensionType Type { get; }
    public PartyMemberInfo[]? Party { get; }
    public GuildMemberInfo[]? Guild { get; }
    internal ProtocolExtensionEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Type = (ProtocolExtensionType)pvSrc.ReadByte();
        switch (Type)
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
                Party = party.ToArray();
                party.Clear();
                break;

            case ProtocolExtensionType.GuildTrack:
                List<GuildMemberInfo> guild = new();
                while (pvSrc.ReadInt32() != 0x00)
                {
                    pvSrc.Seek(-4, SeekOrigin.Current);
                    guild.Add(GuildMemberInfo.Instantiate(pvSrc));
                }
                Guild = guild.ToArray();
                guild.Clear();
                break;

            default:
                pvSrc.Trace();
                return;
        }
    }
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