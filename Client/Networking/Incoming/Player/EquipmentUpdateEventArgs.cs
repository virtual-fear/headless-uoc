namespace Client.Networking.Incoming;
using Client.Game.Data;
public sealed class EquipUpdateEventArgs : EventArgs
{
    public NetState State { get; }
    public int Item { get; }
    public int ID { get; }
    public Layer Layer { get; }
    public int Mobile { get; }
    public short Hue { get; }
    internal EquipUpdateEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Item = ip.ReadInt32();
        ID = ip.ReadInt16() & 0x3FFF;
        ip.Seek(1, SeekOrigin.Current);
        Layer = (Layer)ip.ReadByte();
        Mobile = ip.ReadInt32();
        Hue = ip.ReadInt16();
    }
}