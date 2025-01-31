namespace Client.Networking.Arguments;
using Client.Networking.Data;
using Client.Networking.Packets;
public sealed class ShardListEventArgs : EventArgs
{
    [PacketHandler(0xA8, length: -1, ingame: false)]
    public static event PacketEventHandler<ShardListEventArgs>? Update;
    public NetState State { get; }
    public ShardEntry[]? ShardEntries { get; }
    internal ShardListEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        // OutgoingAccountPackets.cs : SendAccountLoginAck(this NetState ns);
        var unknown_0x5D = ip.ReadByte();
        ShardEntries = new ShardEntry[ip.ReadUInt16()];
        if (ShardEntries.Length > 0)
        {
            Logger.Log("[Servers]");
            for (int i = 0; i < ShardEntries.Length; i++)
            {
                var entry = new ShardEntry(
                    index: ip.ReadUInt16(),
                     name: ip.ReadStringSafe(32),
                     percentFull: ip.ReadByte(),
                        timeZone: ip.ReadByte(),
                         address: ip.ReadUInt32());

                ShardEntries[i] = entry;
                Logger.Log($"  ({i + 1}) {entry.Name} ({entry.PercentFull}%)", LogColor.Info);
            }
        }
    }
    static ShardListEventArgs() => Update += ShardListEventArgs_Update;
    private static void ShardListEventArgs_Update(ShardListEventArgs e)
    {
        ShardList.View.Shards = e.ShardEntries;

        // Automatically connect to the first shard when Godot is not being used!
        if (Application.Instance == null)
        {
            if (e.ShardEntries?.Length == 0)
            {
                Logger.Log("No server entries are currently available", LogColor.Warning);
                return;
            }
            var shard = e.ShardEntries?.FirstOrDefault();
            if (shard.HasValue == false)
            {
                Logger.Log("No servers are currently available", LogColor.Warning);
                return;
            }
            Logger.Log("  > Connecting to the first shard available!", LogColor.Info);
            if (Network.State == null)
            {
                throw new InvalidOperationException("Invalid network state",
                    innerException: new ArgumentNullException(nameof(Network.State)));
            }

            Network.State.Send(PPlayServer.Instantiate((byte)shard.Value.Index));
        }
    }
}