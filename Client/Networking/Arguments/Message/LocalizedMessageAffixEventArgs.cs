namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;

public sealed class LocalizedMessageAffixEventArgs : EventArgs
{
    [PacketHandler(0xCC, length: -1, ingame: true)]
    private static event PacketEventHandler<LocalizedMessageAffixEventArgs>? Update;
    public NetState State { get; }
    public IEntity Entity { get; }
    public short Graphic { get; }
    public byte MessageType { get; }
    public short Hue { get; }
    public short Font { get; }
    public byte AffixType { get; }
    public string? Name { get; }
    public string? Text { get; }
    public string? Arguments { get; }
    private LocalizedMessageAffixEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial serial = (Serial)ip.ReadUInt32();
        if (serial.IsMobile)
            Entity = Mobile.Acquire(serial);
        else
            Entity = Item.Acquire(serial);
        Graphic = ip.ReadInt16();
        MessageType = ip.ReadByte();
        Hue = ip.ReadInt16();
        Font = ip.ReadInt16();
        ip.ReadInt32();  //  e.Number
        //e.Lang = Localization.GetString(pvSrc.ReadInt32());
        AffixType = ip.ReadByte();
        Name = ip.ReadString(30);
        Text = ip.ReadString();
        Arguments = ip.ReadUnicodeString();
    }

    static LocalizedMessageAffixEventArgs() => Update += LocalizedMessageAffixEventArgs_Update;
    private static void LocalizedMessageAffixEventArgs_Update(LocalizedMessageAffixEventArgs e)
        => Message.Add(e.State, e.Entity, e.Graphic, e.MessageType, e.Hue, e.Font, e.Name, e.Text, e.Arguments);
}