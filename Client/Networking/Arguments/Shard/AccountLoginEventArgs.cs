using ShardData = Client.Networking.Data.ShardData;
namespace Client.Networking.Arguments;
public sealed class AccountLoginEventArgs : EventArgs
{
    public NetState State { get; }
    public AccountLoginEventArgs(NetState state) => State = state;
    public IEnumerable<ShardData>? Shards { get; set; }
}