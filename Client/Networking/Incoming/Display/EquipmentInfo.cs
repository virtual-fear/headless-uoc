namespace Client.Networking.Incoming.Display;
public partial class PacketHandlers
{
    public static event PacketEventHandler<EquipmentInfoEventArgs>? DisplayEquipmentInfo;
    public sealed class EquipmentInfoEventArgs : EventArgs
    {
        private List<EquipInfoAttribute> m_Attributes = new();
        public NetState? State { get; }
        public EquipmentInfoEventArgs(NetState state) => State = state;
        public IEnumerable<EquipInfoAttribute> Attributes => m_Attributes;
        public int Item { get; set; }
        public int Number { get; set; }
        public bool Identified { get; set; }
        public string? Name { get; set; }
        public void AddAttribute(EquipInfoAttribute attribute)
        {
            if (attribute == null)
                return;

            m_Attributes.Add(attribute);
        }
    }
    protected static class EquipmentInfo
    {
        public const bool ExtendedCommand = true;

        [PacketHandler(0x10, length: -1, ingame: true, extCmd: ExtendedCommand)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            EquipmentInfoEventArgs e = new(ns);

            e.Item = pvSrc.ReadInt32();
            e.Number = pvSrc.ReadInt32();
            e.Identified = (pvSrc.ReadInt32() != -3);

            if (e.Identified)
                e.Name = pvSrc.ReadString(pvSrc.ReadUInt16());

            int n, c;
            while (pvSrc.ReadInt32() != -1)
            {
                pvSrc.Seek(-4, SeekOrigin.Current);

                n = pvSrc.ReadInt32();
                c = pvSrc.ReadInt16();

                e.AddAttribute(new EquipInfoAttribute(n, c));
            }
            DisplayEquipmentInfo?.Invoke(e);
        }
    }
}