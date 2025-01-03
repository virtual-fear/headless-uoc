using System;

namespace Client.Networking.IO
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
        public abstract Int32 Count { get; }
    }
}
