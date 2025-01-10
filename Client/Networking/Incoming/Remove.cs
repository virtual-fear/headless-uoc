using Client.Game.Data;

namespace Client.Networking.Incoming;
public partial class PacketHandlers
{
    public static event PacketEventHandler<RemoveEventArgs>? OnRemove;
    public sealed class RemoveEventArgs : EventArgs
    {
        public NetState State { get; }
        public RemoveEventArgs(NetState state) => State = state;
        public Serial Serial { get; set; }
    }
    protected static class Remove
    {
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            RemoveEventArgs e = new(ns);
            e.Serial = (Serial)pvSrc.ReadUInt32();
            OnRemove?.Invoke(e);
        }
    }
}
