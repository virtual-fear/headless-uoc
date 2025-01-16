using CharInfo = Client.Game.Data.CharInfo;
using CityInfo = Client.Game.Data.CityInfo;
namespace Client.Networking.Incoming;
public sealed class CharacterListEventArgs : EventArgs
{
    public NetState State { get; }
    public IEnumerable<CharInfo>? Characters { get; }
    public IEnumerable<CityInfo>? Cities { get; }
    public int Flags { get; }
    internal CharacterListEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Characters = CharInfo.Instantiate(state, ip);
        Cities = CityInfo.Instantiate(ip);
        Flags = ip.ReadInt32(); // CharacterListFlags
        ip.ReadInt16();         // (ushort)-1
    }
}