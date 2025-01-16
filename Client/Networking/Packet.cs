namespace Client.Networking;
using Client.Diagnostics;
public class Packet
{
    public byte ID { get; }
    public bool Fixed { get; }
    public bool Encode { get; set; } = false;
    public Int64 Length => Stream == null ? 0 : Stream.Length;
    public PacketWriter Stream { get; }
    public Packet(byte packetID, int length = 0)
    {
        // 0x00 : packetID
        // 0x01 :   length << 8 (short #2)
        // 0x02 :   length << 0 (short #1)

        ID = packetID;

        if (Fixed = (length <= 0))
            length = 32;

        Stream = new PacketWriter(capacity: length);
        Stream.Write((byte)packetID);
        if (Fixed)
            Stream.Write((ushort)0);

        Type t = this.GetType();
        PacketSendProfile.Acquire(t).Increment();
    }
    public byte[] Compile()
    {
        if (Fixed)
        {
            var packetLength = Stream.Index;
            Stream.Seek(1, SeekOrigin.Begin);
            Stream.Write((ushort)packetLength);
            Stream.Seek(packetLength, SeekOrigin.Begin);
        }
        return Stream.Compile();
    }
    public override string ToString() => $"{GetType().Name} (0x{ID:X2}, {Length})";
}
