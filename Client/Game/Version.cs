using Client.Networking;
using Client.Networking.Packets;

namespace Client.Game;
static class Version
{
    [PacketHandler(0xBE, length: 7, ingame: false)]
    internal static void AssistVer(NetState ns, PacketReader ip) => PAssistantVersion.SendBy(ns, ip);

    [PacketHandler(0xBD, length: 3, ingame: false)]
    internal static void ClientVer(NetState ns, PacketReader ip) => PClientVersion.SendBy(ns);
}
