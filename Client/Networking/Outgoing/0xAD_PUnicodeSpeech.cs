using Client.Game;

namespace Client.Networking.Outgoing
{
    public enum MessageType
    {
        Regular     = 0x00,
        System      = 0x01,
        Emote       = 0x02,
        Label       = 0x06,
        Focus       = 0x07,
        Whisper     = 0x08,
        Yell        = 0x09,
        Spell       = 0x0A,
        Guild       = 0x0D,
        Alliance    = 0x0E,
        Command     = 0x0F,
        Encoded     = 0xC0
    }

    public sealed class PUnicodeSpeech : Packet
    {
        private PUnicodeSpeech()
            : base(0xAD) { }

        public static Packet Instantiate(string text) => Instantiate(text, 0, MessageType.Regular);
        public static Packet Instantiate(string text, short hue, MessageType type)
        {
            Packet packet = new PUnicodeSpeech();
            packet.Stream.Write((byte)type);
            packet.Stream.Write((short)hue);
            packet.Stream.Fill(sizeof(short)); // Font: RunUO doesn't read this
            packet.Stream.Write(Localization.Language);
            bool encode = (type & MessageType.Encoded) != 0x0;
            if (!encode)
            {
                packet.Stream.Write(text);
            }
            else
            {
                // Unsure of this in the RunUO code
            }
            return packet;
        }
    }
}
