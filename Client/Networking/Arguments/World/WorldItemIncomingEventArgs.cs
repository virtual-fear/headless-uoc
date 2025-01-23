using Client.Game;
using Layer = Client.Game.Data.Layer;
using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Arguments;
public sealed class WorldItemIncomingEventArgs : EventArgs
{
    public static event PacketEventHandler<WorldItemIncomingEventArgs>? Update;
    //[PacketHandler(????, length: -1, ingame: true)]
    internal static void Received_WorldIncomingItem(NetState ns, PacketReader ip) => Update?.Invoke(new(ns, ip));

    // Might have to fix this, because we use it for MobileIncoming
    public NetState State { get; }
    public Serial Serial { get; }
    public int ItemID { get; }
    public Layer Layer { get; }
    public short Hue { get; }
    private WorldItemIncomingEventArgs(NetState state, PacketReader ip)
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

    static WorldItemIncomingEventArgs() => Update += WorldItemIncomingEventArgs_Update;
    private static void WorldItemIncomingEventArgs_Update(WorldItemIncomingEventArgs e) => Item.Acquire(e.Serial).Update(e);
}