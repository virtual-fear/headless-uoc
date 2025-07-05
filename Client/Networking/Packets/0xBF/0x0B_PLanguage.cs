using System.Globalization;

namespace Client.Networking.Packets
{
    public class PLanguage : Packet
    {
        public static readonly string Current = CultureInfo.CurrentUICulture.ThreeLetterWindowsLanguageName.ToUpper();
        private PLanguage()
            : base(0xBF) => base.Stream.Write((short)0x0B);

        public static void SendBy(NetState state) => state.Send(PLanguage.Instantiate());
        private static Packet Instantiate()
        {
            Packet packet = new PLanguage();
            packet.Stream.WriteAscii(PLanguage.Current, 4);
            return packet;
        }
    }
}
