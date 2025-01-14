namespace Client.Networking;
using System.Reflection;
public partial class PacketHandlers
{
    private const bool DEBUG_ATTRIBUTES = false;
    private static PacketHandler[] RegisteredPackets = new PacketHandler[0x100];
    public static void Configure() => RegisterAttributes();

    [PacketHandler(0xBF, -1, ingame: true)]
    protected static void ExtendedCommand(NetState ns, PacketReader ip)
    {
        PacketHandler? ph = GetExtendedHandler((byte)ip.ReadInt16());
        if (ph == null)
        {
            ip.Trace();
            return;
        }
        ph.Receive(ns, ip);
    }
    public static PacketHandler? GetHandler(byte packetID) => RegisteredPackets[packetID];
    public static PacketHandler? GetExtendedHandler(byte packetID) => RegisteredPackets[0xBF][packetID];
    internal static void Register(PacketHandler handler) => RegisteredPackets[handler.PacketID] = handler;
    internal static void RegisterExtended(PacketHandler handler) => RegisteredPackets[0xBF][handler.PacketID] = handler;
    protected internal static void RegisterAttributes()
    {
        // Sort & register extended methods first? i.e 0xBF, 0xF0

        var methods = Assembly.GetExecutingAssembly().GetTypes()
            .SelectMany(t =>  t.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            .Where(m => m.GetCustomAttributes(typeof(PacketHandlerAttribute), false).Length > 0)
            .ToList();

        foreach (var method in methods)
        {
            var attr = (PacketHandlerAttribute)method.GetCustomAttributes(typeof(PacketHandlerAttribute), false)[0];
            var receive = (OnPacketReceive)Delegate.CreateDelegate(typeof(OnPacketReceive), method);
            var handler = new PacketHandler(attr.PacketID, attr.Length, attr.Ingame, receive);

            if (DEBUG_ATTRIBUTES)
                Logger.Log(handler.ToString());
            
            if (attr.ExtendedCommand)
                RegisterExtended(handler);
            else
                Register(handler);
        }
    }
}