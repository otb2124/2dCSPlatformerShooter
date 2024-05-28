using Platformer.Physics;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Platformer.Utils
{
    public sealed class Counter
    {
        private int count;
        private Stopwatch watch;
        private double interval;

        public double CountPerInterval
        {
            get { return count / (watch.Elapsed.TotalSeconds / interval); }
        }

        public double CountTotal
        {
            get { return count; }
        }

        public Counter(float interval)
        {
            interval = FlatMath.Clamp(interval, 0.1f, 10f);

            count = 0;
            watch = new Stopwatch();
            this.interval = interval;
        }

        public void Start()
        {
            watch.Start();
        }

        public void Restart()
        {
            count = 0;
            watch.Restart();
        }

        public void Stop()
        {
            count = 0;
            watch.Stop();
        }

        public void IncCount(int amount = 1)
        {
            count += amount;
        }

    }
}
