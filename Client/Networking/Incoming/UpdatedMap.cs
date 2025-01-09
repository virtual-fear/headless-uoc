using System.Collections;
using Client.Game.Agents;

namespace Client.Networking.Incoming;

using static PacketSink;
public partial class PacketSink
{
    #region EventArgs
    public sealed class MapCommandEventArgs : EventArgs
    {
        public NetState State { get; }
        public MapCommandEventArgs(NetState state) => State = state;
        public int MapItem { get; set; }
        public byte Command { get; set; }
        public byte Number { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
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
    public class MapPatchesEventArgs : EventArgs
    {
        private NetState m_State;
        private PacketReader m_Reader;
        private Hashtable m_Table;

        public NetState State { get { return m_State; } }
        public PacketReader Reader { get { return m_Reader; } set { m_Reader = value; } }
        public Hashtable Table { get { return m_Table; } set { m_Table = value; } }

        public MapPatchesEventArgs(NetState state)
        {
            m_State = state;
        }
    }
    public sealed class MapDetailsEventArgs : EventArgs
    {
        public NetState State { get; }
        public MapDetailsEventArgs(NetState state) => State = state;
        public int MapItem { get; set; }
        public int XStart { get; set; }
        public int YStart { get; set; }
        public int XEnd { get; set; }
        public int YEnd { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
    #endregion (done)

    public static event PacketEventHandler<MapCommandEventArgs>? MapCommand;
    public static event PacketEventHandler<MapChangeEventArgs>? MapChange;
    public static event PacketEventHandler<MapPatchesEventArgs>? MapPatches;
    public static event PacketEventHandler<MapDetailsEventArgs>? MapDetails;
    public static void InvokeMapDetails(MapDetailsEventArgs e) => MapDetails?.Invoke(e);
    public static void InvokeMapPatches(MapPatchesEventArgs e) => MapPatches?.Invoke(e);
    public static void InvokeMapChange(MapChangeEventArgs e) => MapChange?.Invoke(e);
    public static void InvokeMapCommand(MapCommandEventArgs e) => MapCommand?.Invoke(e);

}
public static class UpdatedMap
{
    public static void Configure()
    {
        Register(0x56, 11, true, new OnPacketReceive(MapCommand));
        RegisterExtended(0x08, 06, true, new OnPacketReceive(MapChange));
        RegisterExtended(0x18, 33, true, new OnPacketReceive(MapPatches));
    }

    private static void MapPatches(NetState ns, PacketReader pvSrc)
    {
        MapPatchesEventArgs e = new MapPatchesEventArgs(ns)
        {
            Reader = pvSrc
        };

        int count = pvSrc.ReadInt32();
        int staticBlocks, landBlocks;

        Hashtable t = new Hashtable();
        t = Hashtable.Synchronized(t);

        for (int i = 0; i < count; i++)
        {
            staticBlocks = pvSrc.ReadInt32();
            landBlocks = pvSrc.ReadInt32();

            // Map.[1].Tiles.Patch.[2]
            // [1]  ==  Felucca | Trammel | Ileshenar | Malas
            // [2]  ==  StaticBlock | LandBlock

            t[WorldContext.GetIndex(i)] = new KeyValuePair<int, int>(staticBlocks, landBlocks);
        }

        e.Table = t;

        PacketSink.InvokeMapPatches(e);
    }
    private static void MapChange(NetState ns, PacketReader pvSrc)
    {
        MapChangeEventArgs e = new MapChangeEventArgs(ns)
        {
            Reader = pvSrc
        };

        e.Index = pvSrc.ReadByte();

        PacketSink.InvokeMapChange(e);
    }
    private static void MapDetails(NetState ns, PacketReader pvSrc)
    {
        MapDetailsEventArgs e = new MapDetailsEventArgs(ns);

        e.MapItem = pvSrc.ReadInt32();
        pvSrc.Seek(2, SeekOrigin.Current);
        //pvSrc.ReadInt16();  //  0x193D
        e.XStart = pvSrc.ReadInt16();
        e.YStart = pvSrc.ReadInt16();
        e.XEnd = pvSrc.ReadInt16();
        e.YEnd = pvSrc.ReadInt16();
        e.Width = pvSrc.ReadInt16();
        e.Height = pvSrc.ReadInt16();

        PacketSink.InvokeMapDetails(e);
    }
    private static void MapCommand(NetState ns, PacketReader pvSrc)
    {
        MapCommandEventArgs e = new MapCommandEventArgs(ns);

        e.MapItem = pvSrc.ReadInt32();
        e.Command = pvSrc.ReadByte();
        e.Number = pvSrc.ReadByte();
        e.X = pvSrc.ReadInt16();
        e.Y = pvSrc.ReadInt16();

        PacketSink.InvokeMapCommand(e);
    }
    static void RegisterExtended(byte packetID, int length, bool variable, OnPacketReceive onReceive) => PacketHandlers.RegisterExtended(packetID, length, variable, onReceive);
    static void Register(byte packetID, int length, bool variable, OnPacketReceive onReceive) => PacketHandlers.Register(packetID, length, variable, onReceive);
}
