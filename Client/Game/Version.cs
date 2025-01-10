using Client.Networking;
using Client.Networking.Outgoing;

namespace Client.Game;
static class Version
{
    [PacketHandler(0xBE, length: 7, ingame: true)]
    internal static void AssistVer(NetState ns, PacketReader ip) => AssistantVersion.SendBy(ns, ip);

    [PacketHandler(0xBD, length: 3, ingame: true)]
    internal static void ClientVer(NetState ns, PacketReader ip) => ClientVersion.SendBy(ns);
}
