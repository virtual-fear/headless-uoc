namespace Client.Networking.Arguments;
using Client.Networking.Packets;
public sealed class ClientVersionEventArgs : EventArgs
{
    [PacketHandler(0xBD, length: 3, ingame: false)]
    public static event PacketEventHandler<ClientVersionEventArgs>? OnUpdate;
    public NetState State { get; }
    public string Text { get; }
    internal ClientVersionEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Text = Application.ClientVersion.ToString();
    }
    static ClientVersionEventArgs() => OnUpdate += ClientVersionEventArgs_OnUpdate;
    private static void ClientVersionEventArgs_OnUpdate(ClientVersionEventArgs e) => e.State?.Send(new PClientVersion(e));
}
