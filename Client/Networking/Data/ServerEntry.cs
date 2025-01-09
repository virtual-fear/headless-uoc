namespace Client.Networking.Data;
internal struct ServerEntry
{
    public uint Index { get; }
    public string Name { get; }
    public uint PercentFull { get; }
    public uint TimeZone { get; }
    public uint Address { get; }
    public ServerEntry(uint index, string name, uint percentFull, uint timeZone, uint address)
    {
        Index = index;
        Name = name;
        PercentFull = percentFull;
        TimeZone = timeZone;
        Address = address;
    }
}