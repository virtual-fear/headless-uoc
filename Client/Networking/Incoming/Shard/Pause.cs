namespace Client.Networking.Incoming;
public sealed class PauseEventArgs : EventArgs
{
    public NetState State { get; }
    public bool Supported { get; }
    public PauseEventArgs(NetState state, bool supported)
    {
        State = state;
        Supported = supported;
    }
}
public partial class Shard
{
    public static event PacketEventHandler<PauseEventArgs>? OnPause;

    [PacketHandler(0x33, 02, true)]
    protected static void Received_Pause(NetState ns, PacketReader pvSrc)
    {
        PauseEventArgs e = new(ns, false);
        OnPause?.Invoke(e);
    }
}
