namespace Client.Networking.Incoming.Shard;
using Client.Networking.Data;
public partial class PacketHandlers
{
    public static event PacketEventHandler<ServerListReceivedEventArgs>? Shard_ServerList;
    public sealed class ServerListReceivedEventArgs : EventArgs
    {
        internal ServerEntry[]? ServerListEntries { get; set; }
    }
    protected static class ServerList
    {
        [PacketHandler(0xA8, length: -1, ingame: false)]
        internal static void Update(NetState state, PacketReader pvSrc)
        {
            // OutgoingAccountPackets.cs : SendAccountLoginAck(this NetState ns);
            //writer.Write((byte)0x5D);
            //writer.Write((ushort)info.Length);
            byte flags = pvSrc.ReadByte(); // 0x5D (Unknown)
            ushort count = pvSrc.ReadUInt16(); // info.Length
            List<ServerEntry> entries = new List<ServerEntry>();
            for (ushort i = 0; i < count; i++)
            {
                entries.Add(new ServerEntry(
                    (uint)pvSrc.ReadInt16(),    // i
                    pvSrc.ReadStringSafe(32), // name
                    pvSrc.ReadByte(), // full percent
                    pvSrc.ReadByte(), // time zone
                    pvSrc.ReadUInt32() // raw ip
                ));
            }
            ServerInfo.Instance.Servers = entries.ToArray();
            Logger.Log("Received the list of available servers");
            
            int entryIdx = 1;
            foreach (var entry in ServerInfo.Instance.Servers)
                Logger.Log($"  {entryIdx++}) {entry.Name} ({entry.PercentFull}%)", LogColor.Info);
            
            ServerListReceivedEventArgs e = new()
            {
                ServerListEntries = ServerInfo.Instance.Servers
            };
            Shard_ServerList?.Invoke(e);
        }
    }
}