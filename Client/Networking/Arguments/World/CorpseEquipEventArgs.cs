using Layer = Client.Game.Data.Layer;
using LayerInfo = Client.Game.Data.LayerInfo;
namespace Client.Networking.Arguments;
public sealed class CorpseEquipEventArgs : EventArgs
{
    public NetState State { get; }
    public int Beheld { get; }
    public LayerInfo[]? Layers { get; }
    internal CorpseEquipEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Beheld = ip.ReadInt32();
        Layer layer;
        List<LayerInfo> l = new List<LayerInfo>();
        while ((layer = (Layer)ip.ReadByte()) != Layer.Invalid)
            l.Add(new LayerInfo(layer, ip));
        Layers = l.ToArray();
    }

}