using System;

namespace Client.IO
{
    public interface IConsolidator
    {
        void Enqueue(byte[] buffer, int offset, int length);
    }
    public abstract class BaseQueue : IConsolidator
    {
        public abstract void Enqueue(byte[] buffer, int offset, int length);
        public abstract ArraySegment<byte> Dequeue(int size);
        public abstract void Clear();
        public abstract int Count { get; }
    }
}
