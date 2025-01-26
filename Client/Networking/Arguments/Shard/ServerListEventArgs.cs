namespace Client.Networking.Arguments;

using Client.Networking.Data;
using Client.Networking.Packets;
public sealed class ServerListEventArgs : EventArgs
{
    [PacketHandler(0xA8, length: -1, ingame: false)]
    public static event PacketEventHandler<ServerListEventArgs>? Update;
    public NetState State { get; }
    public ServerEntry[]? Servers { get; }
    internal ServerListEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        // OutgoingAccountPackets.cs : SendAccountLoginAck(this NetState ns);
        var unknown_0x5D = ip.ReadByte();
        Servers = new ServerEntry[ip.ReadUInt16()];
        if (Servers.Length > 0)
        {
            Logger.Log("[Servers]");
            for (int i = 0; i < Servers.Length; i++)
            {
                var entry = new ServerEntry(
                    index: ip.ReadUInt16(),
                     name: ip.ReadStringSafe(32),
                     percentFull: ip.ReadByte(),
                        timeZone: ip.ReadByte(),
                         address: ip.ReadUInt32());

                Servers[i] = entry;
                Logger.Log($"  ({i + 1}) {entry.Name} ({entry.PercentFull}%)", LogColor.Info);
            }
        }
    }
    static ServerListEventArgs() => Update += ServerListEventArgs_Update;
    private static void ServerListEventArgs_Update(ServerListEventArgs e)
    {
        // Automatically connect to the server when Godot is not being used!
        if (Application.Instance == null)
        {
            if (e.Servers?.Length == 0)
            {
                Logger.Log("No server entries are currently available", LogColor.Warning);
                return;
            }
            var shard = e.Servers?.FirstOrDefault();
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