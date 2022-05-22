using System;
using System.Diagnostics;

namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    public class TimeMonitor : ITimeMonitor
    {
        public float DeltaTime { get; private set; }

        private Stopwatch _stopwatch;

        public TimeMonitor()
        {
            Reset();
        }

        public void OnTick()
        {
            _stopwatch.Stop();
            DeltaTime = (float)_stopwatch.Elapsed.TotalSeconds;
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public void Reset()
        {
            _stopwatch ??= new Stopwatch();
            _stopwatch.Stop();
            _stopwatch.Reset();
        }
    }
}
