namespace Client.Game.Data.Other;
using Client.Networking;
using Client.Networking.Outgoing;
public sealed class CharInfo
{
    public NetState State { get; }
    public int Index { get; }
    public string Name { get; }
    private CharInfo(NetState ns, int index, string name)
    {
        State = ns;
        Index = index;
        Name = name;
    }
    public void Play() => State.Send(PPlayCharacter.Instantiate(this));
    public static CharInfo[] Instantiate(NetState state, PacketReader pvSrc)
    {
        List<CharInfo> list = new List<CharInfo>();
        int c = pvSrc.ReadByte();

        for (int i = 0; i < c; i++)
        {
            string un = pvSrc.ReadString(30).TrimEnd('\0');
            pvSrc.ReadString(30);

            if (string.IsNullOrEmpty(un))
                continue;

            list.Add(new CharInfo(state, i, un));
        }

        return list.ToArray();
    }
}