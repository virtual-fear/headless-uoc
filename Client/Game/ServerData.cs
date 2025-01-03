using Client.Networking;

namespace Client.Game
{
    internal class ServerData
    {
        public static ServerData Instance = new ServerData();

        public string Address { get; set; }
        public string Port { get; set; }
        public ServerListEntry[] ServerEntries { get; set; }
        public CharInfo[] CharInfo { get; set; }
        public CityInfo[] CityInfo { get; set; }
    }

    internal class ServerListEntry
    {
        public ServerListEntry(uint index, string name, uint percentFull, uint timeZone, uint address)
        {
            Index = index;
            Name = name;
            PercentFull = percentFull;
            TimeZone = timeZone;
            Address = address;
        }

        public uint Index { get; private set; }
        public string Name { get; private set; }
        public uint PercentFull { get; private set; }
        public uint TimeZone { get; private set; }
        public uint Address { get; private set; }
    }
}