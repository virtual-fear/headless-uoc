namespace Client.Networking.Incoming;

using System.Threading.Tasks.Dataflow;
using Client.Networking.Data;
public sealed class ServerListReceivedEventArgs : EventArgs
{
    public ServerEntry[]? ServerListEntries { get; set; }
}
public partial class Shard
{
    public static event PacketEventHandler<ServerListReceivedEventArgs>? UpdateServerList;

    [PacketHandler(0xA8, length: -1, ingame: false)]
    protected static void Received_ServerList(NetState state, PacketReader pvSrc)
    {
        Logger.PushWarning("Received server list");
        // OutgoingAccountPackets.cs : SendAccountLoginAck(this NetState ns);
        //writer.Write((byte)0x5D);
        //writer.Write((ushort)info.Length);
        byte flags = pvSrc.ReadByte(); // 0x5D (Unknown)
        ushort count = pvSrc.ReadUInt16(); // info.Length
        List<ServerEntry> serverList = new();
        for (ushort i = 0; i < count; i++)
        {
            serverList.Add(new ServerEntry(
                (uint)pvSrc.ReadInt16(),    // i
                pvSrc.ReadStringSafe(32), // name
                pvSrc.ReadByte(), // full percent
                pvSrc.ReadByte(), // time zone
                pvSrc.ReadUInt32() // raw ip
            ));
        }
        ServerInfo.Instance.Servers = serverList.ToArray();
        Logger.Log("Received the list of available servers");

        int entryIdx = 1;
        foreach (var entry in ServerInfo.Instance.Servers)
            Logger.Log($"  {entryIdx++}) {entry.Name} ({entry.PercentFull}%)", LogColor.Info);

        ServerListReceivedEventArgs e = new()
        {
            ServerListEntries = ServerInfo.Instance.Servers
        };
        UpdateServerList?.Invoke(e);
    }
}