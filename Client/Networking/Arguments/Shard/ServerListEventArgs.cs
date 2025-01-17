using ServerEntry = Client.Networking.Data.ServerEntry;
namespace Client.Networking.Arguments;
public sealed class ServerListEventArgs : EventArgs
{
    public NetState State { get; }
    public ServerEntry[]? ServerListEntries { get; }
    internal ServerListEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        // OutgoingAccountPackets.cs : SendAccountLoginAck(this NetState ns);
        var unknown_0x5D = ip.ReadByte();
        ServerListEntries = new ServerEntry[ip.ReadUInt16()];
        if (ServerListEntries.Length > 0)
        {
            Logger.Log("[Servers]");
            for (int i = 0; i < ServerListEntries.Length; i++)
            {
                var entry = new ServerEntry(
                    index: ip.ReadUInt16(),
                     name: ip.ReadStringSafe(32),
                     percentFull: ip.ReadByte(),
                        timeZone: ip.ReadByte(),
                         address: ip.ReadUInt32());

                ServerListEntries[i] = entry;
                Logger.Log($"  ({i + 1}) {entry.Name} ({entry.PercentFull}%)", LogColor.Info);
            }
        }
    }
}