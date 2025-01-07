namespace Client.Networking.Incoming;
using Client.Networking.Outgoing;
public static class UpdatedVersion
{
    public static void Configure()
    {
        Register(0xBE, 07, true, new OnPacketReceive(AssistVer));
        Register(0xBD, 03, false, new OnPacketReceive(ClientVer));
    }
    private static void ClientVer(NetState ns, PacketReader ip) => ClientVersion.SendBy(ns);
    private static void AssistVer(NetState ns, PacketReader ip) => AssistantVersion.SendBy(ns, ip);
    public static void Register(int packetID, int length, bool ingame, OnPacketReceive receive) => PacketHandlers.Register(packetID, length, ingame, receive);
}