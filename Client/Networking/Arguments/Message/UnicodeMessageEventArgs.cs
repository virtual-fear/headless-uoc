namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;

public sealed class UnicodeMessageEventArgs : EventArgs
{
    [PacketHandler(0xAE, length: -1, ingame: true)]
    private static event PacketEventHandler<UnicodeMessageEventArgs>? Update;
    public NetState State { get; }
    public IEntity Entity { get; }
    public short Graphic { get; set; }
    public byte MessageType { get; set; }
    public short Hue { get; set; }
    public short Font { get; set; }
    public string? Language { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
    private UnicodeMessageEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        pvSrc.Seek(0, SeekOrigin.Begin);
        Serial serial = (Serial)pvSrc.ReadUInt32();
        if (serial.IsMobile)
            Entity = Mobile.Acquire(serial);
        else
            Entity = Item.Acquire(serial);
        Graphic = pvSrc.ReadInt16();
        MessageType = pvSrc.ReadByte();
        Hue = pvSrc.ReadInt16();
        Font = pvSrc.ReadInt16();
        Language = pvSrc.ReadString(4);
        Name = pvSrc.ReadString(30);
        Text = pvSrc.ReadUnicodeString();
    }

    static UnicodeMessageEventArgs() => Update += UnicodeMessageEventArgs_Update;
    private static void UnicodeMessageEventArgs_Update(UnicodeMessageEventArgs e)
        => Message.Add(e.State, e.Entity, e.Graphic, e.MessageType, e.Hue, e.Font, e.Language, e.Name, e.Text);
}