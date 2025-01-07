using System;
using System.Collections.Generic;
namespace Client.Networking.IO
{
    public sealed class OutputQueue : BaseQueue, IConsolidator
    {
        private object Chain = new object();
        private List<byte[]> Queue = new List<byte[]>();
        private bool IsBusy = false;
        public override void Enqueue(byte[] buffer, int offset, int length) { lock (Chain) Queue.Add(buffer); }
        public override ArraySegment<byte> Dequeue(int size) => throw new NotSupportedException();
        public override void Clear()
        {
            lock (Chain)
            {
                Queue.Clear();
                IsBusy = false;
            }
        }
        public override int Count => Queue.Count;
        public byte[]? Proceed()
        {
            lock (Chain)
            {
                IsBusy = true;
                try
                {
                    if (Queue.Count > 0)
                    {
                        var buff = Queue[0];
                        Queue.RemoveAt(0);
                        IsBusy = false;
                        return buff;
                    }
                }
                finally
                {
                    if (Queue.Count == 0)
                        IsBusy = false;
                }
                return null;
            }
        }
        public byte[]? Query()
        {
            lock (Chain)
            {
                if (IsBusy || Queue.Count <= 0)
                {
                    return null;
                }
                IsBusy = true;
                return Queue[0];
            }
        }
    }
}
