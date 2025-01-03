namespace Client.IO
{
    public sealed class Collector<T>
    {
        private object Chain { get; } = new object();
        private object Items { get; } = new object();
        public List<T> Queue { get; } = new List<T>();
        public int Count { get { return Queue.Count; } }
        public T this[int i] => Queue[i];
        public void Clear() { lock (Chain) Queue.Clear(); }
        public T[] Dequeue(int count)
        {
            lock (Chain)
            {
                if (count > Count)
                    count = Count;

                T[] array = new T[count];
                for (int i = 0; i < count; ++i)
                {
                    array[i] = Queue[0];
                    Queue.RemoveAt(0);
                }
                return array;
            }
        }
        public void Enqueue(T item) { lock (Items) Queue.Add(item); }
        public void Enqueue(T[] items, int count)
        {
            lock (Chain)
            {
                if (count > items.Length)
                    count = items.Length;

                for (int i = 0; i < count; ++i)
                    Enqueue(items[i]);
            }
        }
    }
}
