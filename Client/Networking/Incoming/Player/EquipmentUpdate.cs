namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class EquipUpdateEventArgs : EventArgs
{
    public NetState State { get; }
    public EquipUpdateEventArgs(NetState state) => State = state;
    public int Item { get; set; }
    public int ID { get; set; }
    public Layer Layer { get; set; }
    public int Mobile { get; set; }
    public short Hue { get; set; }
}
public partial class Player
{
    public static event PacketEventHandler<EquipUpdateEventArgs>? OnEquipmentUpdate;

    [PacketHandler(0x2E, length: 15, ingame: true)]
    protected static void Receive_EquipmentUpdate(NetState ns, PacketReader pvSrc)
    {
        EquipUpdateEventArgs e = new(ns);
        e.Item = pvSrc.ReadInt32();
        e.ID = pvSrc.ReadInt16() & 0x3FFF;
        pvSrc.Seek(1, SeekOrigin.Current);
        e.Layer = (Layer)pvSrc.ReadByte();
        e.Mobile = pvSrc.ReadInt32();
        e.Hue = pvSrc.ReadInt16();
        OnEquipmentUpdate?.Invoke(e);
    }
}