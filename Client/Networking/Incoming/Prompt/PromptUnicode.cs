namespace Client.Networking.Incoming.Prompt;
public partial class PacketHandlers
{
    public static event PacketEventHandler<PromptUnicodeEventArgs>? Prompt_Unicode;
    public sealed class PromptUnicodeEventArgs : EventArgs
    {
        public NetState State { get; }
        public PromptUnicodeEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public int Prompt { get; set; }
    }
    protected static class PromptUnicode
    {
        [PacketHandler(0xC2, length: -1, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            PromptUnicodeEventArgs e = new(ns);
            int v = pvSrc.ReadInt32();
            e.Serial = v;
            e.Prompt = v;
            pvSrc.ReadInt32();  //  (copy of serial)
            pvSrc.ReadInt32();  //  0
            pvSrc.ReadInt32();  //  0
            pvSrc.ReadInt16();  //  0
            Prompt_Unicode?.Invoke(e);
        }
    }
}
