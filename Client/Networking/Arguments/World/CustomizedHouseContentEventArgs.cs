namespace Client.Networking.Arguments;
using Client.Game;
public sealed class CustomizedHouseContentEventArgs : EventArgs
{
    [PacketHandler(0xD8, length: -1, ingame: true)]
    private static event PacketEventHandler<CustomizedHouseContentEventArgs>? Update;
    public NetState State { get; }
    public bool Supported { get; } = false;
    public int CompressionType { get; }
    public int Serial { get; }
    public int Revision { get; }
    public byte[]? Buffer { get; }
    private CustomizedHouseContentEventArgs(NetState state, PacketReader ip)
    {
        State = state;
        CompressionType = ip.ReadByte();
        ip.ReadByte();
        Serial = ip.ReadInt32();
        Revision = ip.ReadInt32();
        ip.ReadUInt16();
        int length = ip.ReadUInt16();
        Buffer = ip.ReadBytes(length);
    }

    static CustomizedHouseContentEventArgs() => Update += CustomizedHouseContentEventArgs_Update;
    private static void CustomizedHouseContentEventArgs_Update(CustomizedHouseContentEventArgs e) => World.UpdateHouseContent(e);
}