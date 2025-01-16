using Layer = Client.Game.Data.Layer;
using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class WorldItemIncomingEventArgs : EventArgs
{
    // Might have to fix this, because we use it for MobileIncoming
    public NetState State { get; }
    public Serial Serial { get; }
    public int ItemID { get; }
    public Layer Layer { get; }
    public short Hue { get; }
    internal WorldItemIncomingEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        Serial = (Serial)ip.ReadUInt32();
        if (Serial != 0x00)
        {
            ItemID = ip.ReadUInt16();
            Layer = (Layer)ip.ReadByte();
            Hue = ip.ReadInt16();
        }
    }
}