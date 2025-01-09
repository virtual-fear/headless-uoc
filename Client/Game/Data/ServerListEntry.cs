namespace Client.Game.Data;
internal sealed class ServerListEntry
{
    public uint Index { get; }
    public string Name { get; }
    public uint PercentFull { get; }
    public uint TimeZone { get; }
    public uint Address { get; }
    public ServerListEntry(uint index, string name, uint percentFull, uint timeZone, uint address)
    {
        Index = index;
        Name = name;
        PercentFull = percentFull;
        TimeZone = timeZone;
        Address = address;
    }
}