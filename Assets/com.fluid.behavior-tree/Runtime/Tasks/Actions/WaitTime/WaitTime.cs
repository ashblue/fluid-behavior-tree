namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    /// <summary>
    ///     Return continue until the time has passed
    /// </summary>
    public class WaitTime : ActionBase
    {
        private readonly ITimeMonitor _timeMonitor;
        private float _timePassed;

        public float time = 1;

        public WaitTime(ITimeMonitor timeMonitor)
        {
            _timeMonitor = timeMonitor;
        }

        public override string IconPath { get; } = $"{PACKAGE_ROOT}/Hourglass.png";

        protected override void OnStart()
        {
            _timePassed = 0;
        }

        protected override TaskStatus OnUpdate()
        {
            _timePassed += _timeMonitor.DeltaTime;

            if (_timePassed < time) return TaskStatus.Continue;

            return TaskStatus.Success;
        }
    }
}
