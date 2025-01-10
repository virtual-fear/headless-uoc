namespace Client.Networking.Incoming.Shard;
using Client.Networking.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<AccountLoginEventArgs>? Shard_AccountLogin;
    public sealed class AccountLoginEventArgs : EventArgs
    {
        public NetState? State { get; }
        public AccountLoginEventArgs(NetState? state) => State = state;
        public IEnumerable<ShardData>? Shards { get; set; }
    }

    protected static class AccountLogin
    {
    }
}
