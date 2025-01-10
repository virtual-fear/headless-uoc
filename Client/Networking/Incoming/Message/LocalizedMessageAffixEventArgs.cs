namespace Client.Networking.Incoming;
public sealed class LocalizedMessageAffixEventArgs : EventArgs
{
    public NetState State { get; }
    public LocalizedMessageAffixEventArgs(NetState state) => State = state;
    public int Serial { get; set; }
    public short Graphic { get; set; }
    public byte MessageType { get; set; }
    public short Hue { get; set; }
    public short Font { get; set; }
    public byte AffixType { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
    public string? Arguments { get; set; }
}
public partial class Message
{
    public static event PacketEventHandler<LocalizedMessageAffixEventArgs>? OnLocalizedAffix;

    [PacketHandler(0xCC, length: -1, ingame: true)]
    protected static void ReceivedMessage_LocalizedAffix(NetState ns, PacketReader pvSrc)
    {
        LocalizedMessageAffixEventArgs e = new(ns);

        e.Serial = pvSrc.ReadInt32();
        e.Graphic = pvSrc.ReadInt16();
        e.MessageType = pvSrc.ReadByte();
        e.Hue = pvSrc.ReadInt16();
        e.Font = pvSrc.ReadInt16();

        pvSrc.ReadInt32();  //  e.Number

        //e.Lang = Localization.GetString(pvSrc.ReadInt32());

        e.AffixType = pvSrc.ReadByte();
        e.Name = pvSrc.ReadString(30);
        e.Text = pvSrc.ReadString();
        e.Arguments = pvSrc.ReadUnicodeString();
        OnLocalizedAffix?.Invoke(e);
    }
}