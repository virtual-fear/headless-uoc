namespace Client.Networking.Incoming.Prompt;
public partial class PacketHandlers
{
    public static event PacketEventHandler<PromptAsciiEventArgs>? Prompt_Ascii;
    public sealed class PromptAsciiEventArgs : EventArgs
    {
        public NetState State { get; }
        public PromptAsciiEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public int Prompt { get; set; }
        public string? Text { get; set; }
    }
    protected static class PromptAscii
    {
        [PacketHandler(0x9A, length: -1, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            PromptAsciiEventArgs e = new(ns);

            int v = pvSrc.ReadInt32();

            e.Serial = v;
            e.Prompt = v;

            pvSrc.ReadInt32();  //  (copy of serial)
            pvSrc.ReadInt32();  //  0

            e.Text = pvSrc.ReadString().Trim();
            Prompt_Ascii?.Invoke(e);
        }
    }
}
