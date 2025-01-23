using Client.Networking.Packets;
using ServerEntry = Client.Networking.Data.ServerEntry;
namespace Client.Networking.Arguments;
public sealed class ServerListEventArgs : EventArgs
{
    [PacketHandler(0xA8, length: -1, ingame: false)]
    private static event PacketEventHandler<ServerListEventArgs>? Update;
    public NetState State { get; }
    public ServerEntry[]? Shards { get; }
    internal ServerListEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        // OutgoingAccountPackets.cs : SendAccountLoginAck(this NetState ns);
        var unknown_0x5D = ip.ReadByte();
        Shards = new ServerEntry[ip.ReadUInt16()];
        if (Shards.Length > 0)
        {
            Logger.Log("[Servers]");
            for (int i = 0; i < Shards.Length; i++)
            {
                var entry = new ServerEntry(
                    index: ip.ReadUInt16(),
                     name: ip.ReadStringSafe(32),
                     percentFull: ip.ReadByte(),
                        timeZone: ip.ReadByte(),
                         address: ip.ReadUInt32());

                Shards[i] = entry;
                Logger.Log($"  ({i + 1}) {entry.Name} ({entry.PercentFull}%)", LogColor.Info);
            }
        }
    }
    static ServerListEventArgs() => Update += ServerListEventArgs_Update;
    private static void ServerListEventArgs_Update(ServerListEventArgs e)
    {
        // Automatically connect to the server because we aren't using Godot here!
        if (Application.Instance == null)
        {
            if (e.Shards.Length == 0)
            {
                Logger.Log("No shard entries are currently available", LogColor.Warning);
                return;
            }
            var shard = e.Shards.FirstOrDefault();
            if (shard.Name.Length == 0)
            {
                Logger.Log("No shards are currently available", LogColor.Warning);
                return;
            }
            Logger.Log("  > Connecting to the first shard available!", LogColor.Info);
            if (Network.State == null)
            {
                throw new InvalidOperationException("Invalid network state",
                    innerException: new ArgumentNullException(nameof(Network.State)));
            }
            Network.State.Send(PPlayServer.Instantiate((byte)shard.Index));
        }
    }
}