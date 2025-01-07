namespace Client.Networking;
using Client.Diagnostics;
public class Packet
{
    public byte ID { get; }
    public bool Fixed { get; }
    public bool Encode { get; set; } = true;
    public Int64 Length => Stream == null ? 0 : Stream.Length;
    public PacketWriter Stream { get; }
    public Packet(byte packetID, int length = -1)
    {
        // 0x00 : packetID
        // 0x01 :   length << 8 (short #2)
        // 0x02 :   length << 0 (short #1)

        ID = packetID;

        if (Fixed = (length <= 0))
            length = 32;
        
        Stream = new PacketWriter(length);
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
            Stream.Seek(1L, SeekOrigin.Begin);
            Stream.Write((ushort)Stream.Length);
        }
        Stream.Flush();
        return Stream.Compile();
    }
    public override string ToString() => $"{GetType().Name} (0x{ID:X2}, {Length})";
}
