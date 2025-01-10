namespace Client.Networking.Incoming.Map;
public partial class PacketHandlers
{
    public static event PacketEventHandler<MapChangeEventArgs>? MapUpdateChange;
    public class MapChangeEventArgs : EventArgs
    {
        private NetState m_State;
        private PacketReader m_Reader;
        private byte m_Index;

        public NetState State { get { return m_State; } }
        public PacketReader Reader { get { return m_Reader; } set { m_Reader = value; } }
        public byte Index { get { return m_Index; } set { m_Index = value; } }

        public MapChangeEventArgs(NetState state)
        {
            m_State = state;
        }
    }

    protected static class MapChange
    {
        [PacketHandler(0x8, length: 6, ingame: true, extCmd: true)]
        public static void Update(NetState ns, PacketReader pvSrc)
        {
            MapChangeEventArgs e = new MapChangeEventArgs(ns)
            {
                Reader = pvSrc,
                Index = pvSrc.ReadByte()
            };
            MapUpdateChange?.Invoke(e);
        }
    }
}
