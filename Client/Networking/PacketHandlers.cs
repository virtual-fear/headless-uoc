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
    protected internal static void RegisterAttributeEvents()
    {
        const BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        var types = Assembly.GetExecutingAssembly().GetTypes();
        var events = types.SelectMany(t => t.GetEvents(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                          .Where(e => e.GetCustomAttributes(typeof(PacketHandlerAttribute), false).Length > 0)
                          .ToList();

        foreach (var eventInfo in events)
        {
            var attr = (PacketHandlerAttribute)eventInfo.GetCustomAttributes(typeof(PacketHandlerAttribute), false)[0];
            var handlerType = eventInfo.EventHandlerType;
            if (handlerType == null)
            {
                Logger.PushWarning($"Event {eventInfo.Name} has invalid handler type.");
                continue;
            }

            var classType = eventInfo.DeclaringType;
            if (classType == null)
            {
                Logger.PushWarning($"Event {eventInfo.Name} has invalid declared type.");
                continue;
            }

            var eventArgs = eventInfo?.EventHandlerType?.GetGenericArguments()[0];
            var eventInvoker = delegate (NetState ns, PacketReader ip)
            {
                var fieldName = eventInfo?.Name;
                if (fieldName == null)
                    throw new ArgumentNullException(nameof(fieldName));
                var eventField = classType.GetField(fieldName, bindingAttr);
                if (eventField != null)
                {
                    var eventDelegate = (MulticastDelegate?)eventField.GetValue(null);
                    if (eventDelegate != null)
                    {
                        var eventType = Activator.CreateInstance(eventArgs,
                                bindingAttr: BindingFlags.Instance |
                                             BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.DeclaredOnly,
                                binder: null,
                                args: new object[] { ns, ip },
                                culture: null);
                        // Invoke the EventArgs to our event
                        foreach (Delegate d in eventDelegate.GetInvocationList())
                            d.Method.Invoke(d.Target, parameters: new object[] { eventType });
                    }
                }
            };

            var eventName = $"{classType.Namespace?.Replace("Client.", string.Empty)}" +
                           $"{classType.Name}." +
                           $"{eventInfo?.Name ?? $"(unknown:0x{attr.PacketID:X2})"}";

            var ph = new PacketHandler(attr.PacketID, attr.Length, attr.Ingame,
                receive: new((ns, ip) => eventInvoker.Invoke(ns, ip)),
                name: eventName);

            if (attr.ExtendedCommand)
                RegisterExtended(ph);
            else
                Register(ph);
        }
    }
}