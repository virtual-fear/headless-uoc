namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class CorpseEquipEventArgs : EventArgs
{
    public NetState State { get; }
    public CorpseEquipEventArgs(NetState state) => State = state;
    public int Beheld { get; set; }
    public LayerInfo[]? Layers { get; set; }
}
public partial class World
{
    public static event PacketEventHandler<CorpseEquipEventArgs>? UpdateCorpseEquip;

    [PacketHandler(0x89, length: -1, ingame: true)]
    internal static void Received_CorpseEquip(NetState ns, PacketReader pvSrc)
    {
        CorpseEquipEventArgs e = new(ns);
        e.Beheld = pvSrc.ReadInt32();
        Layer layer;
        List<LayerInfo> l = new List<LayerInfo>();
        while ((layer = (Layer)pvSrc.ReadByte()) != Layer.Invalid)
            l.Add(new LayerInfo(layer, pvSrc));
        e.Layers = l.ToArray();
        UpdateCorpseEquip?.Invoke(e);
    }
}
