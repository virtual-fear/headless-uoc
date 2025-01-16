namespace Client.Networking.Incoming;
public sealed class SpellbookContentEventArgs : EventArgs
{
    public NetState State { get; }
    public int Item { get; }
    public short Graphic { get; }
    public short Offset { get; }
    public long Content { get; }
    internal SpellbookContentEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadInt16();  //  0x01
        Item = ip.ReadInt32();
        Graphic = ip.ReadInt16();
        Offset = ip.ReadInt16();
        long content = 0;
        for (int i = 0; i < 8; ++i)
            content += ip.ReadByte() << (i * 8);
        // todo: fix the content
        Content = content;
    }
}