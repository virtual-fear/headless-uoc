namespace Client.Networking.Arguments;
using System.Collections;
using Client.Game;
using Client.Game.Data;
public sealed class MapPatchesEventArgs : EventArgs
{
    [PacketHandler(0x18, length: 33, ingame: true, extCmd: true)]
    private static event PacketEventHandler<MapPatchesEventArgs>? Update;
    public NetState State { get; }
    public Hashtable? Table { get; }
    private MapPatchesEventArgs(NetState state, PacketReader pvSrc)
    {
        State = state;
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
        Table = t;
    }

    static MapPatchesEventArgs() => Update += MapPatchesEventArgs_Update;
    private static void MapPatchesEventArgs_Update(MapPatchesEventArgs e)
        => Map.Patches(e.State, e.Table);
}