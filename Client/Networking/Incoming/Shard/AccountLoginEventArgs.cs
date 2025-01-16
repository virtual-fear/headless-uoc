using ShardData = Client.Networking.Data.ShardData;
namespace Client.Networking.Incoming;
public sealed class AccountLoginEventArgs : EventArgs
{
    public NetState State { get; }
    public AccountLoginEventArgs(NetState state) => State = state;
    public IEnumerable<ShardData>? Shards { get; set; }
}