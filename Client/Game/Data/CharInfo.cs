namespace Client.Game.Data;
using Client.Networking;
public sealed class CharInfo
{
    private sealed class PlayCharacter : Packet
    {
        private PlayCharacter() : base(0x5D, 73) { }
        public static Packet Instantiate(CharInfo ci)
        {
            if (ci == null)
                throw new ArgumentNullException("Unable to play character, slot is invalid.", "info");

            Packet p = new PlayCharacter();
            PacketWriter s = p.Stream;

            s.Write(0xEDEDEDED);
            s.WriteAsciiFixed(ci.Name, 30);

            s.Seek(02, SeekOrigin.Current);
            s.Write((int)31);

            s.Seek(24, SeekOrigin.Current);
            s.Write(ci.Index);

            NetState ns = ci.State;
            s.Write(ns.AuthID);

            Console.WriteLine("Packet:PlayCharacter AuthID: {0}", ns.AuthID);

            return p;
        }
    }

    public NetState State { get; }
    private CharInfo(NetState state, int index, string name)
    {
        State = state;
        Index = index;
        Name = name;
    }
    public int Index { get; }
    public string Name { get; }
    public void Play() => State.Send(PlayCharacter.Instantiate(this));
    public static CharInfo[] Instantiate(NetState state, PacketReader pvSrc)
    {
        List<CharInfo> list = new List<CharInfo>();
        int c = pvSrc.ReadByte();

        for (int i = 0; i < c; i++)
        {
            string un = pvSrc.ReadString(30).TrimEnd('\0');
            pvSrc.ReadString(30);

            if (string.IsNullOrEmpty(un))
            {
                continue;
            }

            list.Add(new CharInfo(state, i, un));
        }

        return list.ToArray();
    }
}
