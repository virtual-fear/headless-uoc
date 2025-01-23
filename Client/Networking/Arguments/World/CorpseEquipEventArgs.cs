namespace Client.Networking.Arguments;
using Game.Data;
public sealed class CorpseEquipEventArgs : EventArgs
{
    [PacketHandler(0x89, length: -1, ingame: true)]
    private static event PacketEventHandler<CorpseEquipEventArgs>? Update;
    public NetState State { get; }
    public int Beheld { get; }
    public LayerInfo[]? Layers { get; }
    private CorpseEquipEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Beheld = ip.ReadInt32();
        Layer layer;
        List<LayerInfo> l = new List<LayerInfo>();
        while ((layer = (Layer)ip.ReadByte()) != Layer.Invalid)
            l.Add(new LayerInfo(layer, ip));
        Layers = l.ToArray();
    }

    static CorpseEquipEventArgs() => Update += CorpseEquipEventArgs_Update;
    private static void CorpseEquipEventArgs_Update(CorpseEquipEventArgs e) => Game.World.CorpseEquip(e);
}