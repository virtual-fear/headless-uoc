using Client.Networking.Data;

namespace Client.Networking.Arguments;
public sealed class AccountLoginEventArgs : EventArgs
{
    public static event PacketEventHandler<AccountLoginEventArgs>? Update;
    public NetState State { get; }
    public AccountLoginEventArgs(NetState state) => State = state;
    public ShardEntry[]? Shards { get; set; }
    static AccountLoginEventArgs()
    {

    }
}