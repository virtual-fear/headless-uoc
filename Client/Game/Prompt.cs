namespace Client.Game;
using Client.Networking;
using Client.Networking.Arguments;
public static class Prompt
{
    [PacketHandler(0x9A, length: -1, ingame: true)]
    public static event PacketEventHandler<PromptAsciiEventArgs>? UpdateASCII;

    [PacketHandler(0xC2, length: -1, ingame: true)]
    public static event PacketEventHandler<PromptUnicodeEventArgs>? OnUnicode;
}
