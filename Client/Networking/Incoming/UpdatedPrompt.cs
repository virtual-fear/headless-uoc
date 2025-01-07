namespace Client.Networking.Incoming;
using static PacketSink;
public partial class PacketSink
{
    #region EventArgs

    public sealed class PromptAsciiEventArgs : EventArgs
    {
        public NetState State { get; }
        public PromptAsciiEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public int Prompt { get; set; }
        public string Text { get; set; }
    }
    public sealed class PromptUnicodeEventArgs : EventArgs
    {
        public NetState State { get; }
        public PromptUnicodeEventArgs(NetState state) => State = state;
        public int Serial { get; set; }
        public int Prompt { get; set; }
    }

    #endregion (done)

    public static event PacketEventHandler<PromptUnicodeEventArgs>? PromptUnicode;
    public static event PacketEventHandler<PromptAsciiEventArgs>? PromptAscii;
    public static void InvokePromptAscii(PromptAsciiEventArgs e) => PromptAscii?.Invoke(e);
    public static void InvokePromptUnicode(PromptUnicodeEventArgs e) => PromptUnicode?.Invoke(e);
}
public static class UpdatedPrompt
{
    public static void Configure()
    {

        Register(0xC2, -1, true, new OnPacketReceive(PromptUnicode));
        Register(0x9A, -1, true, new OnPacketReceive(PromptAscii));
    }
    private static void PromptAscii(NetState ns, PacketReader pvSrc)
    {
        PromptAsciiEventArgs e = new PromptAsciiEventArgs(ns);

        int v = pvSrc.ReadInt32();

        e.Serial = v;
        e.Prompt = v;

        pvSrc.ReadInt32();  //  (copy of serial)
        pvSrc.ReadInt32();  //  0

        e.Text = pvSrc.ReadString().Trim();

        PacketSink.InvokePromptAscii(e);

    }

    private static void PromptUnicode(NetState ns, PacketReader pvSrc)
    {
        PromptUnicodeEventArgs e = new PromptUnicodeEventArgs(ns);

        int v = pvSrc.ReadInt32();

        e.Serial = v;
        e.Prompt = v;

        pvSrc.ReadInt32();  //  (copy of serial)
        pvSrc.ReadInt32();  //  0
        pvSrc.ReadInt32();  //  0
        pvSrc.ReadInt16();  //  0

        PacketSink.InvokePromptUnicode(e);
    }

    private static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive)
    {
        PacketHandlers.Register(packetID, length, variable, onReceive);
    }
}
