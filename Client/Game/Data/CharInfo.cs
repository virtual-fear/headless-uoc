namespace Client.Game.Data;
using Client.Networking;
using Client.Networking.Packets;
public sealed class CharInfo
{
    public NetState State { get; }
    public int Index { get; }
    public string Name { get; }
    CharInfo(NetState state, int index, string name)
    {
        State = state;
        Index = index;
        Name = name;
    }
    public void Play() => State.Send(PPlayCharacter.Instantiate(this));
    public static CharInfo[] Construct(NetState state, PacketReader pvSrc)
    {
        List<CharInfo> list = new();
        int c = pvSrc.ReadByte();
        for (int idx = 0; idx < c; idx++)
        {
            string un = pvSrc.ReadString(30).TrimEnd('\0');
            pvSrc.ReadString(30);
            if (string.IsNullOrEmpty(un)) continue;
            list.Add(new CharInfo(state, idx, un));
        }
        return list.ToArray();
    }
}