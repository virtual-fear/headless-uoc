namespace Client.Networking.Incoming;
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
    public ContainerContentUpdateEventArgs(NetState state) => m_State = state;
}
public partial class Container
{
    public static event PacketEventHandler<ContainerContentUpdateEventArgs>? OnContentUpdate;

    [PacketHandler(0x25, length: 20, ingame: true)]
    protected static void ReceivedContainer_ContentUpdate(NetState ns, PacketReader pvSrc)
    {
        ContainerContentUpdateEventArgs e = new(ns);
        e.Serial = pvSrc.ReadInt32();
        e.ID = pvSrc.ReadUInt16();
        pvSrc.ReadByte();   //  signed, itemID offset
        e.Amount = pvSrc.ReadUInt16();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();
        e.Parent = pvSrc.ReadInt32();
        e.Hue = pvSrc.ReadInt16();
        OnContentUpdate?.Invoke(e);
    }
}