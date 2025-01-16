namespace Client.Networking.Incoming;
public sealed class PromptUnicodeEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get; }
    public int Prompt { get; }
    internal PromptUnicodeEventArgs(NetState state, PacketReader ip) {
        State = state;
        Serial = ip.ReadInt32();
        Prompt = ip.ReadInt32(); // (copy of serial)
        ip.ReadInt32();  //  0
        ip.ReadInt32();  //  0
        ip.ReadInt16();  //  0
    }
}