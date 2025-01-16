namespace Client.Networking.Incoming;
public sealed class LocalizedMessageAffixEventArgs : EventArgs
{
    public NetState State { get; }
    public int Serial { get; }
    public short Graphic { get; }
    public byte MessageType { get; }
    public short Hue { get; }
    public short Font { get; }
    public byte AffixType { get;}
    public string? Name { get;  }
    public string? Text { get;  }
    public string? Arguments { get; }
    internal LocalizedMessageAffixEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = ip.ReadInt32();
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
}