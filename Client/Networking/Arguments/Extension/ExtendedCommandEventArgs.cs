namespace Client.Networking.Arguments;
public sealed class ExtendedCommandEventArgs : EventArgs
{
    [PacketHandler(0xBF, length: -1, ingame: true)]
    private static event PacketEventHandler<ExtendedCommandEventArgs>? Update;
    public NetState State { get; }
    public byte PacketID { get; }
    public PacketHandler? Handler { get; }
    private PacketReader Input { get; }
    private ExtendedCommandEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        PacketID = (byte)ip.ReadInt16();
        Handler = PacketHandlers.GetExtendedHandler(PacketID);
        Input = ip;
    }
    static ExtendedCommandEventArgs() => Update += ExtendedCommandEventArgs_Receive;
    private static void ExtendedCommandEventArgs_Receive(ExtendedCommandEventArgs eventArgs)
    {
        if (eventArgs == null)
            throw new ArgumentNullException(nameof(eventArgs));

        if (eventArgs.Handler is var h)
        {
            if (h == null)
            {
                eventArgs.Input.Trace();
                return;
            }
            h.Receive(state: eventArgs.State, ip: eventArgs.Input);
        }
    }
}