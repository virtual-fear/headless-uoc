namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public static class Message
{
    [PacketHandler(0x1C, length: -1, ingame: true)]
    public static event PacketEventHandler<AsciiMessageEventArgs>? OnASCII;

    [PacketHandler(0xCC, length: -1, ingame: true)]
    public static event PacketEventHandler<LocalizedMessageAffixEventArgs>? OnLocalizedAffix;

    [PacketHandler(0xC1, length: -1, ingame: true)]
    public static event PacketEventHandler<LocalizedMessageEventArgs>? OnLocalized;

    [PacketHandler(0xA6, length: -1, ingame: true)]
    public static event PacketEventHandler<ScrollMessageEventArgs>? Chat_OnScrollMessage;

    [PacketHandler(0xAE, length: -1, ingame: true)]
    public static event PacketEventHandler<UnicodeMessageEventArgs>? OnUnicode;
}