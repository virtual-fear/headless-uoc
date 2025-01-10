namespace Client.Networking.Incoming;
using Client.Networking.Data;
public sealed class AccountLoginEventArgs : EventArgs
{
    public NetState State { get; }
    public AccountLoginEventArgs(NetState state) => State = state;
    public IEnumerable<ShardData>? Shards { get; set; }
}

public partial class Shard
{
    public static event PacketEventHandler<AccountLoginEventArgs>? OnAccountLogin;
}
