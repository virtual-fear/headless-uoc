namespace Client.Networking.Incoming.Shard;
public partial class PacketHandlers
{
    public static event PacketEventHandler<LoginCompleteEventArgs>? Shard_LoginComplete;
    public sealed class LoginCompleteEventArgs : EventArgs
    {
        public NetState State { get; }
        public LoginCompleteEventArgs(NetState state) => State = state;
    }

    protected static class LoginComplete
    {

        [PacketHandler(0x55, length: 1, ingame: true)]
        private static void Update(NetState ns, PacketReader pvSrc)
        {
            LoginCompleteEventArgs e = new(ns);
            Shard_LoginComplete?.Invoke(e);
        }
    }
}