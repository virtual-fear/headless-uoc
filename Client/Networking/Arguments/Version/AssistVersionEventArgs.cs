namespace Client.Networking.Arguments;
using Client.Networking.Packets;
public sealed class AssistVersionEventArgs : EventArgs
{
    [PacketHandler(0xBE, length: 7, ingame: false)]
    public static event PacketEventHandler<AssistVersionEventArgs>? OnUpdate;
    public NetState State { get; } 
    public int Version { get; }
    public string Text { get; }
    internal AssistVersionEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Version = ip.ReadInt32();
        Text = Application.ClientVersion.ToString();
    }
    static AssistVersionEventArgs() => OnUpdate += AssistVersionEventArgs_OnUpdate;
    private static void AssistVersionEventArgs_OnUpdate(AssistVersionEventArgs e) => e.State?.Send(new PAssistantVersion(e));
}