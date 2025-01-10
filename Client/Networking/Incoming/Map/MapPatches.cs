namespace Client.Networking.Incoming;
using System.Collections;
using Client.Game.Data;
public sealed class MapPatchesEventArgs : EventArgs
{
    public NetState State { get; }
    public PacketReader? Reader { get; set; }
    public Hashtable? Table { get; set; }
    public MapPatchesEventArgs(NetState state) => State = state;
}
public partial class Map
{
    public static event PacketEventHandler<MapPatchesEventArgs>? OnPatch;

    [PacketHandler(0x18, length: 33, ingame: true, extCmd: true)]
    protected static void ReceivedMap_Patches(NetState ns, PacketReader pvSrc)
    {
        MapPatchesEventArgs e = new(ns) { Reader = pvSrc };
        int staticBlocks, landBlocks;
        Hashtable t = new Hashtable();
        t = Hashtable.Synchronized(t);
        int count = pvSrc.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            staticBlocks = pvSrc.ReadInt32();
            landBlocks = pvSrc.ReadInt32();
            // Map.[1].Tiles.Patch.[2]
            // [1]  ==  Felucca | Trammel | Ileshenar | Malas
            // [2]  ==  StaticBlock | LandBlock
            t[(WorldType)i] = new KeyValuePair<int, int>(staticBlocks, landBlocks);
        }
        e.Table = t;
        OnPatch?.Invoke(e);
    }
}