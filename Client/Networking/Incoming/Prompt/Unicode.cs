namespace Client.Networking.Incoming;
public sealed class PromptUnicodeEventArgs : EventArgs
{
    public NetState State { get; }
    public PromptUnicodeEventArgs(NetState state) => State = state;
    public int Serial { get; set; }
    public int Prompt { get; set; }
}

public partial class Prompt
{
    public static event PacketEventHandler<PromptUnicodeEventArgs>? OnUnicode;

    [PacketHandler(0xC2, length: -1, ingame: true)]
    protected static void Received_Unicode(NetState ns, PacketReader pvSrc)
    {
        PromptUnicodeEventArgs e = new(ns);
        int v = pvSrc.ReadInt32();
        e.Serial = v;
        e.Prompt = v;
        pvSrc.ReadInt32();  //  (copy of serial)
        pvSrc.ReadInt32();  //  0
        pvSrc.ReadInt32();  //  0
        pvSrc.ReadInt16();  //  0
        OnUnicode?.Invoke(e);
    }
}
