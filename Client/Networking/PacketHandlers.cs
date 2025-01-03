namespace Client.Networking
{
    using System;
    using Incoming;
    public partial class PacketHandlers
    {
        private static PacketHandler[] RegisteredPacket = new PacketHandler[0x100];
        public static void Configure()
        {
            Register(0xBF, -1, true, new OnPacketReceive(ExtendedCommand));
            UpdatedServer.RegisterHandlers();
        }
        private static void ExtendedCommand(NetState ns, PacketReader ip)
        {
            PacketHandler handler = GetExtendedHandler(ip.ReadInt16());
            if (handler == null)
            {
                ip.Trace();
                return;
            }
            handler.Receive(ns, ip);
        }
        public static PacketHandler GetHandler(int packetID) => RegisteredPacket[packetID];
        public static PacketHandler GetExtendedHandler(int packetID) => GetHandler(0xBF)[packetID];
        public static void Register(int packetID, int length, bool ingame, OnPacketReceive receive) => RegisteredPacket[packetID] = new PacketHandler(packetID, length, ingame, receive);
        public static void RegisterExtended(int packetID, int length, bool ingame, OnPacketReceive receive) => RegisteredPacket[0xBF][packetID] = new PacketHandler(packetID, length, ingame, receive);
        public static void RemoveHandler(int packetID) => RegisteredPacket[packetID] = null;
        public static void RemoveExtendedHandler(int packetID) => RegisteredPacket[0xBF][packetID] = null;
    }
}