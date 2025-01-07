namespace Client.Networking.Outgoing;
using Client.Game;
using Client.Game.Data;
public sealed class PUnicodeSpeech : Packet
{
    private PUnicodeSpeech() : base(0xAD) { }
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