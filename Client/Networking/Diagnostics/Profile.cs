using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Client.Networking.Diagnostics
{
    using Game;

    public class Profile
    {
        private static readonly bool Enabled = false;

        private static Dictionary<Type, Profile> _profiles = new Dictionary<Type, Profile>();
        public static IEnumerable<Profile> Profiles { get { return _profiles.Values; } }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Profile Acquire(Type type)
        {
            Profile prof = null;

            if (!_profiles.TryGetValue(type, out prof))
            {
                _profiles.Add(type, prof = new Profile(type));
            }

            return prof;
        }

        private static readonly Comparison<Profile> m_Comparer;

        public static void WriteAll<T>(TextWriter op, IEnumerable<T> profiles)
            where T : Profile
        {
            List<T> list = new List<T>(profiles);

            list.Sort(m_Comparer);

            foreach (T prof in list)
            {
                prof.WriteTo(op);
                op.WriteLine();
            }
        }

        private string _name;
        private Stopwatch _stopwatch;
        private TimeSpan _totalTime;
        private TimeSpan _peakTime;
        private long _count, _created;

        public string Name { get { return _name; } }
        public TimeSpan AverageTime { get { return TimeSpan.FromTicks(_totalTime.Ticks / Math.Max(1, _count)); } }
        public TimeSpan TotalTime { get { return _totalTime; } }
        public TimeSpan PeakTime { get { return _peakTime; } }
        public long Count { get { return _count; } }

        public Profile(string name)
        {
            _name = name;

            _stopwatch = new Stopwatch();
        }

        public Profile(Type type)
            : this(type.FullName)
        {
        }

        public void Increment()
        {
            if (Enabled)
            {
                Interlocked.Increment(ref _created);
            }
        }

        public virtual void Start()
        {
            if (_stopwatch.IsRunning)
                _stopwatch.Reset();

            _stopwatch.Start();
        }

        public virtual void Stop()
        {
            TimeSpan elapsed = _stopwatch.Elapsed;

            _stopwatch.Reset();

            _totalTime += elapsed;

            if (elapsed > _peakTime)
                _peakTime = elapsed;

            _count++;
        }

        public virtual void WriteTo(TextWriter op)
        {
            op.Write("{0,-100} {1,12:N0} {2,12:F5} {3,-12:F5} {4,12:F5}", _name, _count, AverageTime.TotalSeconds, _peakTime, _totalTime);
            op.Write("\t({0,12:N0}", _created);
        }

        static Profile()
        {
            m_Comparer = new Comparison<Profile>(Sort);
        }

        private static int Sort(Profile l, Profile r)
        {
            return -l.TotalTime.CompareTo(r.TotalTime);
        }
    }
}
