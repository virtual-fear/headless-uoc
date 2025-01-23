using Client.Networking.Data;
namespace Client.Networking.Arguments;
internal sealed class ServerAckEventArgs : EventArgs
{
    [PacketHandler(0x8C, length: 11, ingame: false)]
    private static event PacketEventHandler<ServerAckEventArgs>? Update;
    public NetState State { get; }
    public uint Addr { get; }
    public short Port { get; }
    public uint Seed { get; }
    private ServerAckEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Addr = ip.ReadUInt32LE();
        Port = ip.ReadInt16();
        Seed = ip.ReadUInt32();
    }
    static ServerAckEventArgs() => Update += ServerAckEventArgs_Update;
    private static async void ServerAckEventArgs_Update(ServerAckEventArgs e)
    {
        Logger.Log(typeof(Assistant), "Received acknowledgement from the server.", color: LogColor.Info);
        Logger.Log($"{new string(' ', nameof(Assistant).Length + -4)}Seed:0x{e.Seed:X4}", color: LogColor.Success);
        var info = Network.Info;
        info.Seed = e.Seed;
        info.Stage = ConnectionAck.SecondLogin;
        Network.Info = info;
        Network.Socket?.Disconnect(reuseSocket: true);
        await Task.CompletedTask;
    }
}