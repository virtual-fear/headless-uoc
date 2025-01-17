namespace Client.Networking;
using System.Reflection;
public partial class PacketHandlers
{
    private const bool DEBUG_ATTRIBUTES = false;
    private static PacketHandler[] RegisteredPackets = new PacketHandler[0x100];
    //public static void Configure() => RegisterAttributes();
    public static PacketHandler? GetHandler(byte packetID) => RegisteredPackets[packetID];
    public static PacketHandler? GetExtendedHandler(byte packetID) => RegisteredPackets[0xBF][packetID];
    internal static void Register(PacketHandler handler) => RegisteredPackets[handler.PacketID] = handler;
    internal static void RegisterExtended(PacketHandler handler) => RegisteredPackets[0xBF][handler.PacketID] = handler;
    protected internal static void RegisterAttributeEvents()
    {
        const BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        var types = Assembly.GetExecutingAssembly().GetTypes();
        var events = types.SelectMany(t => t.GetEvents(bindingAttr))
                          .Where(delegate(EventInfo info)
                          {
                              var attributes = info.GetCustomAttributes(typeof(PacketHandlerAttribute), inherit: false);
                              return attributes.Length > 0 && attributes[0] is PacketHandlerAttribute attr;
                          })
                          .ToList();

        events.Sort(new Comparison<EventInfo>(
            delegate(EventInfo x, EventInfo y)
            {
                var attr = (PacketHandlerAttribute?)x.GetCustomAttributes(typeof(PacketHandlerAttribute), inherit: false)[0];
                if (attr == null || attr.ExtendedCommand) return 1;
                if (attr.PacketID switch // Packets using handlers get registered first!
                {
                    0xBF => true, // Extended Command
                    0xF0 => true, // Protocol Extension
                    _ => false
                }) return -1;
                else return 0;
            }));

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

            var eventType = eventInfo?.EventHandlerType?.GetGenericArguments()[0];
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
                        var eventArgs = Activator.CreateInstance(eventType,
                                bindingAttr: BindingFlags.Instance |
                                             BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.DeclaredOnly,
                                binder: null,
                                args: new object[] { ns, ip },
                                culture: null);
                        // Invoke the EventArgs to our event
                        foreach (Delegate d in eventDelegate.GetInvocationList())
                            d.Method.Invoke(d.Target, parameters: new object?[] { eventArgs });
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