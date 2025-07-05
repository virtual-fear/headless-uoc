namespace Client.Networking.Arguments;
using Client.Networking.Packets;
public sealed class AssistVersionEventArgs : EventArgs
{
    [PacketHandler(0xBE, length: 7, ingame: false)]
    private static event PacketEventHandler<AssistVersionEventArgs>? Update;
    public NetState State { get; } 
    public int Version { get; }
    public string Text { get; }
    internal AssistVersionEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Version = ip.ReadInt32();
        Text = Application.ClientVersion.ToString();
    }
    static AssistVersionEventArgs() => Update += AssistVersionEventArgs_Update;
    private static void AssistVersionEventArgs_Update(AssistVersionEventArgs e) => e.State?.Send(new PAssistantVersion(e));
}