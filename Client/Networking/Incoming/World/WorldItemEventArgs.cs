using Serial = Client.Game.Data.Serial;
namespace Client.Networking.Incoming;
public sealed class WorldItemEventArgs : EventArgs
{
    public NetState State { get; }
    public Serial Serial { get; }
    public short ItemID { get; }
    public short Amount { get; }
    public short X { get; }
    public short Y { get; }
    public sbyte Z { get; }
    public short Hue { get; }
    public byte Flags { get; }
    internal WorldItemEventArgs(NetState state, PacketReader ip)
    {
        State = state;

        // Read serial
        uint serial = ip.ReadUInt32();
        bool hasAmount = (serial & 0x80000000) != 0;
        serial &= 0x7FFFFFFF;
        Serial = (Serial)serial;

        // Read item ID
        ushort itemID = ip.ReadUInt16();
        bool isMulti = (itemID & 0x4000) != 0;
        itemID &= 0x3FFF;

        // Read amount if present
        ushort amount = 0;
        if (hasAmount)
            amount = ip.ReadUInt16();

        // Read X and Y coordinates
        ushort x = ip.ReadUInt16();
        ushort y = ip.ReadUInt16();

        bool hasDirection = (x & 0x8000) != 0;
        x &= 0x7FFF;

        bool hasHue = (y & 0x8000) != 0;
        bool hasFlags = (y & 0x4000) != 0;
        y &= 0x3FFF;

        // Read direction if present
        byte direction = 0;
        if (hasDirection)
            direction = ip.ReadByte();

        // Read Z coordinate
        sbyte z = ip.ReadSByte();

        // Read hue if present
        ushort hue = 0;
        if (hasHue)
            hue = ip.ReadUInt16();

        // Read flags if present
        byte flags = 0;
        if (hasFlags)
            flags = ip.ReadByte();
    }
}
