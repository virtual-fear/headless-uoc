namespace Client.IO;
public sealed class InputQueue : BaseQueue, IConsolidator
{
    private Collector<byte> Buffer = new Collector<byte>();
    public override void Enqueue(byte[] buffer, int offset, int length) => Buffer.Enqueue(buffer, length);
    public override ArraySegment<byte> Dequeue(int size) => new(Buffer.Dequeue(size < 0 ? 0 : size));
    public override void Clear() => Buffer.Clear();
    public override int Count => Buffer.Count;
    public byte GetPacketID() => (byte)(Count >= 1 ? Buffer[0] : -1);
    public short GetPacketLength() => (short)(Count >= 3 ? Buffer[1] << 8 | Buffer[2] : -1);
}
