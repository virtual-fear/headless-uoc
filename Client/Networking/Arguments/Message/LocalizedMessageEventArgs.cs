namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;

public sealed class LocalizedMessageEventArgs : EventArgs
{
    [PacketHandler(0xC1, length: -1, ingame: true)]
    private static event PacketEventHandler<LocalizedMessageEventArgs>? Update;
    public NetState State { get; }
    public IEntity Entity { get; }
    public short Graphic { get; }
    public byte MessageType { get; }
    public short Hue { get; }
    public short Font { get; }
    public string? Name { get; }
    public string? Text { get; }
    private LocalizedMessageEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Serial serial = (Serial)pvSrc.ReadUInt32();
        if (serial.IsMobile)
            Entity = Mobile.Acquire(serial);
        else
            Entity = Item.Acquire(serial);
        Graphic = pvSrc.ReadInt16();
        MessageType = pvSrc.ReadByte();
        Hue = pvSrc.ReadInt16();
        Font = pvSrc.ReadInt16();
        pvSrc.ReadInt32();  //  e.Number
        Name = pvSrc.ReadString(30);
        Text = pvSrc.ReadUnicodeStringLE();
    }

    static LocalizedMessageEventArgs() => Update += LocalizedMessageEventArgs_Update;
    private static void LocalizedMessageEventArgs_Update(LocalizedMessageEventArgs e)
        => Message.Add(e.State, e.Entity, e.Graphic, e.MessageType, e.Hue, e.Font, e.Name, e.Text, null);
}