namespace Client.Networking.Arguments;
using Client.Game;
public sealed class DisplayEquipInfoEventArgs : EventArgs
{
    [PacketHandler(0x10, length: -1, ingame: true, extCmd: true)]
    private static event PacketEventHandler<DisplayEquipInfoEventArgs> Update;
    public NetState State { get; }
    public List<EquipInfoAttribute> Attributes { get; }
    public int ItemID { get; }
    public int Number { get; }
    public bool Identified { get; }
    public string Name { get; internal set; }
    private DisplayEquipInfoEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
        Attributes = new List<EquipInfoAttribute>();
        ItemID = pvSrc.ReadInt32();
        Number = pvSrc.ReadInt32();
        Identified = pvSrc.ReadInt32() != -3;

        if (Identified)
            Name = pvSrc.ReadString(pvSrc.ReadUInt16());
        else
            Name = "(unknown-equip)";

        int n, c;
        while (pvSrc.ReadInt32() != -1)
        {
            pvSrc.Seek(-4, SeekOrigin.Current);

            n = pvSrc.ReadInt32();
            c = pvSrc.ReadInt16();

            Attributes.Add(new EquipInfoAttribute(n, c));
        }
    }
    static DisplayEquipInfoEventArgs() => Update += DisplayEquipInfoEventArgs_Update;
    private static void DisplayEquipInfoEventArgs_Update(DisplayEquipInfoEventArgs e)
        => Display.ShowEquipInfo(e.State, e.ItemID, e.Number, e.Identified, e.Name, e.Attributes);
}