namespace Client.Networking.Arguments;
using Client.Game.Data;
public sealed class CharacterListEventArgs : EventArgs
{
    [PacketHandler(0xA9, length: -1, ingame: false)]
    public static event PacketEventHandler<CharacterListEventArgs>? Update;
    public NetState State { get; }
    public IEnumerable<CharInfo>? Characters { get; }
    public IEnumerable<CityInfo>? Cities { get; }
    public int Flags { get; }
    CharacterListEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Characters = CharInfo.Construct(state, ip);
        Cities = CityInfo.Construct(ip);
        Flags = ip.ReadInt32(); // CharacterListFlags
        ip.ReadInt16();         // (ushort)-1
    }
    static CharacterListEventArgs() => Update += CharacterListEventArgs_Update;
    private static void CharacterListEventArgs_Update(CharacterListEventArgs e)
    {
        if (Application.Instance == null)
        {
            CharInfo[]? characterList = e.Characters?.ToArray();
            if (characterList == null || characterList.Length == 0)
            {
                Logger.LogError($"{nameof(Assistant)}: No characters to select.");
                e.State.Detach();
                return;
            }

            CharInfo? firstCharacter = characterList.FirstOrDefault();
            if (firstCharacter == null)
                throw new ArgumentNullException(nameof(firstCharacter));

            firstCharacter.Play();
            Network.State?.Slice();
        }
    }
}