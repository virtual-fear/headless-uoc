using Client.Game.Data;

namespace Client.Networking.Data;
public sealed class ServerInfo
{
    public static ServerInfo Instance = new ServerInfo();
    public string? Address { get; set; }
    public string? Port { get; set; }
    public ServerEntry[]? Servers { get; set; }
    public CharInfo[]? CharInfo { get; set; }
    public CityInfo[]? CityInfo { get; set; }
}