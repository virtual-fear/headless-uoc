namespace Client.Networking.Incoming.Shard;
using Client.Game.Data;
using Client.Networking.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<CharacterListEventArgs>? Shard_CharacterList;
    public sealed class CharacterListEventArgs : EventArgs
    {
        public NetState State { get; }
        public CharacterListEventArgs(NetState state) => State = state;
        public IEnumerable<CharInfo>? Characters { get; private set; }
        public IEnumerable<CityInfo>? Cities { get; private set; }
        public int Flags { get; private set; }
        public void LoadCharacters(PacketReader pvSrc) => Characters = CharInfo.Instantiate(this.State, pvSrc);
        public void LoadCities(PacketReader pvSrc)
        {
            Cities = CityInfo.Instantiate(pvSrc);
            Flags = pvSrc.ReadInt32();  //      CharacterListFlags
            pvSrc.ReadInt16();          //      -1
        }
    }

    protected static class CharacterList
    {
        [PacketHandler(0xA9, length: -1, ingame: false)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            CharacterListEventArgs e = new CharacterListEventArgs(ns);
            e.LoadCharacters(pvSrc);
            e.LoadCities(pvSrc);
            ServerInfo.Instance.CharInfo = (CharInfo[])e.Characters;
            ServerInfo.Instance.CityInfo = (CityInfo[])e.Cities;
            Shard_CharacterList?.Invoke(e);
        }
    }
}