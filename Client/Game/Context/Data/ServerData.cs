namespace Client.Game.Context.Data;
internal sealed class ServerData
{
    public static ServerData Instance = new ServerData();
    public string? Address { get; set; }
    public string? Port { get; set; }
    public ServerListEntry[]? ServerEntries { get; set; }
    public CharInfo[]? CharInfo { get; set; }
    public CityInfo[]? CityInfo { get; set; }
}