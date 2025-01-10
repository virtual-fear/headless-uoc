namespace Client.Networking;
using System.Reflection;
partial class PacketHandlers
{
    private static PacketHandler[] RegisteredPacket = new PacketHandler[0x100];
    public static void Configure() => RegisterAttributes();

    [PacketHandler(0xBF, -1, ingame: true)]
    protected static void ExtendedCommand(NetState ns, PacketReader ip)
    {
        PacketHandler? handler = GetExtendedHandler(ip.ReadInt16());
        if (handler == null)
        {
            ip.Trace();
            return;
        }
        handler.Receive(ns, ip);
    }
    public static PacketHandler? GetHandler(int packetID) => RegisteredPacket[packetID];
    public static PacketHandler? GetExtendedHandler(int packetID) => GetHandler(0xBF) is var ext && (ext != null) ? ext[packetID] : null;
    public static void RegisterAttributes()
    {
        // TODO: Register extended methods first!

        var methods = Assembly.GetExecutingAssembly().GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            .Where(m => m.GetCustomAttributes(typeof(PacketHandlerAttribute), false).Length > 0)
            .ToList();

        foreach (var method in methods)
        {
            var attr = (PacketHandlerAttribute)method.GetCustomAttributes(typeof(PacketHandlerAttribute), false)[0];
            var receive = (OnPacketReceive)Delegate.CreateDelegate(typeof(OnPacketReceive), method);
            var handler = new PacketHandler(attr.PacketID, attr.Length, attr.Ingame, receive);

            if (attr.ExtendedCommand)
                RegisterExtended(handler);
            else
                Register(handler);
        }
    }
    protected static void Register(PacketHandler handler) => RegisteredPacket[handler.PacketID] = handler;
    protected static void RegisterExtended(PacketHandler handler) => RegisteredPacket[0xBF][handler.PacketID] = handler;
}