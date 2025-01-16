namespace Client.Networking.Incoming;
public sealed class PromptAsciiEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get; set; }
    public int Prompt { get; set; }
    public string? Text { get; set; }
    internal PromptAsciiEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
        Prompt = ip.ReadInt32();
        ip.ReadInt32(); // 0
        Text = ip.ReadString().Trim();
    }
}