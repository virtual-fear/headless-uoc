namespace Client.Networking.Outgoing;
public sealed class PPartyMessage : Packet
{
    public PPartyMessage(string text) : base(0xBF)
    {
        base.Stream.Write((short)0x06); // sub cmd
        base.Stream.Write((byte)0x04);
        base.Stream.WriteUnicode(text);
        base.Stream.Write((short)0x00); // EOL?
    }
}
