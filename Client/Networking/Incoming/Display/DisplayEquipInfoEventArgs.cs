namespace Client.Networking.Incoming;
public sealed class DisplayEquipInfoEventArgs : EventArgs
{
    public NetState? State { get; }
    public List<EquipInfoAttribute> Attributes { get; }
    public int Item { get; }
    public int Number { get; }
    public bool Identified { get; }
    public string? Name { get; }
    internal DisplayEquipInfoEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Attributes = new List<EquipInfoAttribute>();
        Item = pvSrc.ReadInt32();
        Number = pvSrc.ReadInt32();
        Identified = (pvSrc.ReadInt32() != -3);

        if (Identified)
            Name = pvSrc.ReadString(pvSrc.ReadUInt16());

        int n, c;
        while (pvSrc.ReadInt32() != -1)
        {
            pvSrc.Seek(-4, SeekOrigin.Current);

            n = pvSrc.ReadInt32();
            c = pvSrc.ReadInt16();

            Attributes.Add(new EquipInfoAttribute(n, c));
        }
    }
}