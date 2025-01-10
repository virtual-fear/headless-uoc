using Client.Game.Data;

namespace Client.Networking.Incoming.Player;
public partial class PacketHandlers
{
    public static event PacketEventHandler<EquipUpdateEventArgs>? Player_EquipmentUpdate;
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

    protected static class EquipmentUpdate
    {
        [PacketHandler(0x2E, length: 15, ingame: true)]
        internal static void Update(NetState ns, PacketReader pvSrc)
        {
            EquipUpdateEventArgs e = new(ns);
            e.Item = pvSrc.ReadInt32();
            e.ID = pvSrc.ReadInt16() & 0x3FFF;
            pvSrc.Seek(1, SeekOrigin.Current);
            e.Layer = (Layer)pvSrc.ReadByte();
            e.Mobile = pvSrc.ReadInt32();
            e.Hue = pvSrc.ReadInt16();
            Player_EquipmentUpdate?.Invoke(e);
        }
    }
}