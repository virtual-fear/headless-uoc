namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class RemoveEventArgs : EventArgs
{
    public NetState State { get; }
    public RemoveEventArgs(NetState state) => State = state;
    public Serial Serial { get; set; }
}
public partial class World
{
    public static event PacketEventHandler<RemoveEventArgs>? OnRemove;
    protected static void Received_RemoveObject(NetState ns, PacketReader pvSrc)
    {
        RemoveEventArgs e = new(ns);
        e.Serial = (Serial)pvSrc.ReadUInt32();
        OnRemove?.Invoke(e);
    }
}
