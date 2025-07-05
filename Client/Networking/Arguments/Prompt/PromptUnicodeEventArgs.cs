namespace Client.Networking.Arguments;
using Client.Game;
public sealed class PromptUnicodeEventArgs : EventArgs
{
    [PacketHandler(0xC2, length: -1, ingame: true)]
    private static event PacketEventHandler<PromptUnicodeEventArgs>? Update;
    public NetState State { get; }
    public int Serial { get; }
    public int PromptID { get; }
    private PromptUnicodeEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
        PromptID = ip.ReadInt32(); // (copy of serial)
        ip.ReadInt32();  //  0
        ip.ReadInt32();  //  0
        ip.ReadInt16();  //  0
    }
    static PromptUnicodeEventArgs() => Update += PromptUnicodeEventArgs_Update;
    private static void PromptUnicodeEventArgs_Update(PromptUnicodeEventArgs e) => Prompt.OnUnicode(e.State, e.Serial, e.PromptID);
}