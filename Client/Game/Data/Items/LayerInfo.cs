namespace Client.Game.Data.Items;

using Client.Networking;
public sealed class LayerInfo
{
    public Layer Layer { get; }
    public int Item { get; }
    public LayerInfo(Layer layer, PacketReader pvSrc)
    {
        Layer = layer;
        Item = pvSrc.ReadInt32();
    }
}
