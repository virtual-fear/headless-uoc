using System.Collections.Concurrent;
namespace Client;
public sealed class EventLoopContext : SynchronizationContext
{
    private readonly ConcurrentQueue<Action> _queue;
    private readonly Thread _mainThread;

    public EventLoopContext()
    {
        _queue = new ConcurrentQueue<Action>();
        _mainThread = Thread.CurrentThread;
    }

    public override SynchronizationContext CreateCopy() => new EventLoopContext();

    public void Post(Action d) => _queue.Enqueue(d);

    public override void Post(SendOrPostCallback d, object? state) => _queue.Enqueue(() => d(state));

    public override void Send(SendOrPostCallback d, object? state)
    {
        if (Thread.CurrentThread == _mainThread)
        {
            d(state);
            return;
        }

        AutoResetEvent evt = new AutoResetEvent(false);

        _queue.Enqueue(() =>
        {
            d(state);
            evt.Set();
        });

        evt.WaitOne();
    }

    public void ExecuteTasks()
    {
        if (Thread.CurrentThread != _mainThread)
        {
            throw new Exception("Called EventLoop.ExecuteTasks on incorrect thread!");
        }

        var count = _queue.Count;

        for (int i = 0; i < count; i++)
        {
            if (_queue.TryDequeue(out var a))
                a();
        }
    }
}