namespace Client.Networking.Arguments;
using Client.Game;
public sealed class SpellbookContentEventArgs : EventArgs
{
    [PacketHandler(0x1B, length: 23, ingame: true, extCmd: true)]
    private static event PacketEventHandler<SpellbookContentEventArgs>? Update; // (ext) packetID: 0x1B
    public NetState State { get; }
    public int Item { get; }
    public short Graphic { get; }
    public short Offset { get; }
    public long Content { get; }
    private SpellbookContentEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        ip.ReadInt16();  //  0x01
        Item = ip.ReadInt32();
        Graphic = ip.ReadInt16();
        Offset = ip.ReadInt16();
        long content = 0;
        for (int i = 0; i < 8; ++i)
            content += ip.ReadByte() << i * 8;
        // todo: fix the content
        Content = content;
    }

    static SpellbookContentEventArgs() => Update += SpellbookContentEventArgs_Update;
    private static void SpellbookContentEventArgs_Update(SpellbookContentEventArgs e)
        => Player.UpdateSpellbook(e.State, e.Item, e.Graphic, e.Offset, e.Content);
}