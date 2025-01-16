namespace Client.Networking.Incoming;
public partial class Prompt
{
    public static event PacketEventHandler<PromptAsciiEventArgs>? UpdateASCII;
    public static event PacketEventHandler<PromptUnicodeEventArgs>? OnUnicode;

    [PacketHandler(0xC2, length: -1, ingame: true)]
    protected static void Received_Unicode(NetState ns, PacketReader ip) => OnUnicode?.Invoke(new(ns, ip));

    [PacketHandler(0x9A, length: -1, ingame: true)]
    protected static void Received_ASCII(NetState ns, PacketReader ip) => UpdateASCII?.Invoke(new(ns, ip));
}
