namespace Client.Networking.Arguments;
using Client.Game;
public sealed class PromptAsciiEventArgs : EventArgs
{
    [PacketHandler(0x9A, length: -1, ingame: true)]
    private static event PacketEventHandler<PromptAsciiEventArgs>? Update;
    public NetState State { get; }
    public int Serial { get; set; }
    public int PromptID { get; set; }
    public string? Text { get; set; }
    private PromptAsciiEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
        PromptID = ip.ReadInt32();
        ip.ReadInt32(); // 0
        Text = ip.ReadString().Trim();
    }
    static PromptAsciiEventArgs() => Update += PromptAsciiEventArgs_Update;
    private static void PromptAsciiEventArgs_Update(PromptAsciiEventArgs e) => Prompt.OnASCII(e.State, e.Serial, e.PromptID, e.Text);
}