using System.Net;

namespace Client.Networking.Packets;
internal sealed class PPlayCharacter : Packet
{
    private PPlayCharacter() : base(0x5D, 73) { Encode = false; }
    public static Packet Instantiate(Game.Data.CharInfo ci)
    {
        if (ci == null)
            throw new ArgumentNullException("Unable to play character, slot is invalid.", "info");
        
        NetState ns = ci.State;
        Packet p = new PPlayCharacter();
        PacketWriter s = p.Stream;
        s.Write((uint)0xEDEDEDED);
        s.WriteAscii(ci.Name, 30); // name
        s.Seek(2, SeekOrigin.Current); // unknown (@36)
        s.Write((int)0x3F); // flags
        s.Seek(24, SeekOrigin.Current);
        s.Write((int)ci.Index); // charSlot
        /**
         *  NOTICE: PlayCharacter(state, reader)
         *  Reading the clientIP is commented out in ModernUO.
         *  s.Write((int)Network.ClientIP);
         * */
        var clientIP = Network.ClientIP.GetAddressBytes();
        s.Write(BitConverter.ToInt32(clientIP, 0));
        return p;
    }
}