namespace Client.Networking.Arguments;
using Client.Game;
using Client.Game.Data;
public sealed class EquipUpdateEventArgs : EventArgs
{

    [PacketHandler(0x2E, length: 15, ingame: true)]
    private static event PacketEventHandler<EquipUpdateEventArgs>? Update;
    public NetState State { get; }
    public int Item { get; }
    public int ID { get; }
    public Layer Layer { get; }
    public int Mobile { get; }
    public short Hue { get; }
    private EquipUpdateEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Item = ip.ReadInt32();
        ID = ip.ReadInt16() & 0x3FFF;
        ip.Seek(1, SeekOrigin.Current);
        Layer = (Layer)ip.ReadByte();
        Mobile = ip.ReadInt32();
        Hue = ip.ReadInt16();
    }

    static EquipUpdateEventArgs() => Update += EquipUpdateEventArgs_Update;
    private static void EquipUpdateEventArgs_Update(EquipUpdateEventArgs e)
        => Player.EquipItem(e.Mobile, e.Item, e.ID, e.Layer, e.Hue);
}