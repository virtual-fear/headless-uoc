namespace Client.Networking.Outgoing;

using Client.Game.Data;

internal sealed class PPlayCharacter : Packet
{
    private PPlayCharacter() : base(0x5D, 73) { }
    public static Packet Instantiate(CharInfo ci)
    {
        if (ci == null)
            throw new ArgumentNullException("Unable to play character, slot is invalid.", "info");

        Packet p = new PPlayCharacter();
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