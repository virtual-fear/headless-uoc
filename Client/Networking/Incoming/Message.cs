namespace Client.Networking.Incoming;
public partial class Message
{
    public static event PacketEventHandler<AsciiMessageEventArgs>? OnASCII;
    public static event PacketEventHandler<LocalizedMessageAffixEventArgs>? OnLocalizedAffix;
    public static event PacketEventHandler<LocalizedMessageEventArgs>? OnLocalized;
    public static event PacketEventHandler<ScrollMessageEventArgs>? Chat_OnScrollMessage;
    public static event PacketEventHandler<UnicodeMessageEventArgs>? OnUnicode;

    [PacketHandler(0xAE, length: -1, ingame: true)]
    protected static void ReceivedMessage_Unicode(NetState ns, PacketReader pvSrc) => OnUnicode?.Invoke(new(ns, pvSrc));

    [PacketHandler(0xA6, length: -1, ingame: true)]
    protected static void ReceivedMessage_Scroll(NetState ns, PacketReader pvSrc) => Chat_OnScrollMessage?.Invoke(new(ns, pvSrc));

    [PacketHandler(0xC1, length: -1, ingame: true)]
    protected static void ReceivedMessage_Localized(NetState ns, PacketReader pvSrc) => OnLocalized?.Invoke(new(ns, pvSrc));

    [PacketHandler(0xCC, length: -1, ingame: true)]
    protected static void ReceivedMessage_LocalizedAffix(NetState ns, PacketReader ip) => OnLocalizedAffix?.Invoke(new(ns, ip));

    [PacketHandler(0x1C, length: -1, ingame: true)]
    protected static void ReceivedMessage_ASCII(NetState ns, PacketReader ip) => OnASCII?.Invoke(new(ns, ip));
}
