namespace Client.Networking.Incoming;
public sealed class SpellbookContentEventArgs : EventArgs
{
    public NetState State { get; }
    public SpellbookContentEventArgs(NetState state) => State = state;
    public int Item { get; set; }
    public short Graphic { get; set; }
    public short Offset { get; set; }
    public long Content { get; set; }
}
public partial class Player
{
    public static event PacketEventHandler<SpellbookContentEventArgs>? UpdateSpellbookContent; // (ext) packetID: 0x1B

    [PacketHandler(0x1B, length: 23, ingame: true, extCmd: true)]
    protected static void Receive_SpellbookContent(NetState ns, PacketReader pvSrc)
    {
        SpellbookContentEventArgs e = new SpellbookContentEventArgs(ns);
        pvSrc.ReadInt16();  //  0x01
        e.Item = pvSrc.ReadInt32();
        e.Graphic = pvSrc.ReadInt16();
        e.Offset = pvSrc.ReadInt16();
        long content = 0;
        for (int i = 0; i < 8; ++i)
            content += pvSrc.ReadByte() << (i * 8);
        // todo: fix the content
        e.Content = content;
        UpdateSpellbookContent?.Invoke(e);
    }
}