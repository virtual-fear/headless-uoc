namespace Client.Networking.Incoming;

using static PacketSink;
public partial class PacketSink
{
    #region EventArgs

    public sealed class ContainerDisplayEventArgs : EventArgs
    {
        public NetState State { get; }
        public ContainerDisplayEventArgs(NetState state) => State = state;
        public int Container { get; set; }
        public short GumpID { get; set; }
    }
    public sealed class ContainerContentUpdateEventArgs : EventArgs
    {
        private NetState m_State;
        private int m_Serial;
        private ushort m_ID;
        private ushort m_Amount;
        private short m_X, m_Y;
        private int m_ParentSerial;
        private short m_Hue;

        public NetState State { get { return m_State; } }
        public int Serial { get { return m_Serial; } set { m_Serial = value; } }
        public ushort ID { get { return m_ID; } set { m_ID = value; } }
        public ushort Amount { get { return m_Amount; } set { m_Amount = value; } }
        public short X { get { return m_X; } set { m_X = value; } }
        public short Y { get { return m_Y; } set { m_Y = value; } }
        public int Parent { get { return m_ParentSerial; } set { m_ParentSerial = value; } }
        public short Hue { get { return m_Hue; } set { m_Hue = value; } }

        public ContainerContentUpdateEventArgs(NetState state)
        {
            m_State = state;
        }
    }
    public sealed class ContainerContentEventArgs : EventArgs
    {
        public NetState State { get; }
        public ContainerContentEventArgs(NetState state) => State = state;
        public ContainerItem[] Items { get; set; }
    }

    #endregion (done)

    public static event PacketEventHandler<ContainerDisplayEventArgs> ContainerDisplay;
    public static event PacketEventHandler<ContainerContentUpdateEventArgs> ContainerContentUpdate;
    public static event PacketEventHandler<ContainerContentEventArgs> ContainerContent;
    public static void InvokeContainerContent(ContainerContentEventArgs e) => ContainerContent?.Invoke(e);
    public static void InvokeContainerContentUpdate(ContainerContentUpdateEventArgs e) => ContainerContentUpdate?.Invoke(e);
    public static void InvokeContainerDisplay(ContainerDisplayEventArgs e) => ContainerDisplay?.Invoke(e);
}

public sealed class ContainerItem
{
    public NetState State { get; }
    public ContainerItem(NetState state) => State = state;
    public int Serial { get; set; } // Child item serial
    public ushort ID { get; set; }
    public ushort Amount { get; set; }
    public short X { get; set; }
    public short Y { get; set; }
    public int Parent { get; set; } // Beheld item serial
    public ushort Hue { get; set; }
}
public static class UpdatedContainer
{
    public static void Configure()
    {
        Register(0x24, 07, true, new OnPacketReceive(ContainerDisplay));
        Register(0x25, 20, true, new OnPacketReceive(ContainerContentUpdate));
        Register(0x3C, -1, true, new OnPacketReceive(ContainerContent));
    }
    private static void ContainerContent(NetState ns, PacketReader pvSrc)
    {
        ContainerContentEventArgs e = new ContainerContentEventArgs(ns);
        ContainerItem[] items = new ContainerItem[pvSrc.ReadUInt16()];
        for (int i = 0; i < items.Length; ++i)
        {
            ContainerItem ci = new ContainerItem(ns);
            ci.Serial = pvSrc.ReadInt32();
            ci.ID = pvSrc.ReadUInt16();
            pvSrc.ReadByte();   //  0   :   signed, itemID offset
            ci.Amount = pvSrc.ReadUInt16();
            ci.X = pvSrc.ReadInt16();
            ci.Y = pvSrc.ReadInt16();
            ci.Parent = pvSrc.ReadInt32();
            ci.Hue = pvSrc.ReadUInt16();
            items[i] = ci;
        }
        e.Items = items;
        PacketSink.InvokeContainerContent(e);
    }
    private static void ContainerContentUpdate(NetState ns, PacketReader pvSrc)
    {
        ContainerContentUpdateEventArgs e = new ContainerContentUpdateEventArgs(ns);
        e.Serial = pvSrc.ReadInt32();
        e.ID = pvSrc.ReadUInt16();
        pvSrc.ReadByte();   //  signed, itemID offset
        e.Amount = pvSrc.ReadUInt16();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Parent = pvSrc.ReadInt32();
        e.Hue = pvSrc.ReadInt16();
        PacketSink.InvokeContainerContentUpdate(e);
    }
    private static void ContainerDisplay(NetState ns, PacketReader pvSrc)
    {
        ContainerDisplayEventArgs e = new ContainerDisplayEventArgs(ns);
        e.Container = pvSrc.ReadInt32();
        e.GumpID = pvSrc.ReadInt16();
        PacketSink.InvokeContainerDisplay(e);
    }
    private static void Register(byte packetID, int length, bool ingame, OnPacketReceive onReceive) => PacketHandlers.Register(packetID, length, ingame, onReceive);
}
