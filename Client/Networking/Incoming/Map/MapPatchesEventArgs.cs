using Hashtable = System.Collections.Hashtable;
using WorldType = Client.Game.Data.WorldType;
namespace Client.Networking.Incoming;
public sealed class MapPatchesEventArgs : EventArgs
{
    public NetState State { get; }
    public Hashtable? Table { get; }
    internal MapPatchesEventArgs(NetState state, PacketReader pvSrc)
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
}