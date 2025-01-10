namespace Client.Networking.Incoming;
public sealed class PromptAsciiEventArgs : EventArgs
    {
        public NetState State { get; }
        public PromptAsciiEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public int Prompt { get; set; }
        public string? Text { get; set; }
    }
public partial class Prompt
{
    public static event PacketEventHandler<PromptAsciiEventArgs>? UpdateASCII;

    [PacketHandler(0x9A, length: -1, ingame: true)]
    protected static void Received_ASCII(NetState ns, PacketReader pvSrc)
    {
        PromptAsciiEventArgs e = new(ns);
        int v = pvSrc.ReadInt32();
        e.Serial = v;
        e.Prompt = v;
        pvSrc.ReadInt32();  //  (copy of serial)
        pvSrc.ReadInt32();  //  0
        e.Text = pvSrc.ReadString().Trim();
        UpdateASCII?.Invoke(e);
    }
}