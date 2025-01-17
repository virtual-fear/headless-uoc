namespace Client.Networking.Arguments;
public sealed class CustomizedHouseContentEventArgs : EventArgs
{
    public NetState State { get; }
    public bool Supported { get; } = false;
    public int CompressionType { get; }
    public int Serial { get; }
    public int Revision { get; }
    public byte[]? Buffer { get; }
    internal CustomizedHouseContentEventArgs(NetState state, PacketReader ip)
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
}