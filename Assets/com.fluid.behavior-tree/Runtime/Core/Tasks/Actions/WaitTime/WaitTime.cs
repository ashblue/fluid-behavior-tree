using System.IO;
using CleverCrow.Fluid.BTs.Trees.Utils;

namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    /// <summary>
    ///     Return continue until the time has passed
    /// </summary>
    public class WaitTime : ActionBase
    {
        private const float DefaultTime = 1;

        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}Hourglass.png";

        public float Time { get; set; } = DefaultTime;

        private readonly ITimeMonitor _timeMonitor;
        private float _timePassed;

        public WaitTime(ITimeMonitor timeMonitor)
        {
            _timeMonitor = timeMonitor;
        }

        protected override void OnStart()
        {
            _timePassed = MathFExtensions.Zero;
            _timeMonitor.Reset();
        }

        protected override TaskStatus OnUpdate()
        {
            _timeMonitor.OnTick();
            _timePassed += _timeMonitor.DeltaTime;
            return _timePassed < Time ? TaskStatus.Continue : TaskStatus.Success;
        }
    }
}
