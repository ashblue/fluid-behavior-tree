using System;

namespace CleverCrow.Fluid.BTs.Tasks
{
    public class ConditionGeneric : ConditionBase
    {
        public Action ExitLogic { get; set; }
        public Action InitLogic { get; set; }
        public Action StartLogic { get; set; }
        public Func<bool> UpdateLogic { get; set; }

        protected override bool OnUpdate() => UpdateLogic == null || UpdateLogic();

        protected override void OnStart() => StartLogic?.Invoke();

        protected override void OnExit() => ExitLogic?.Invoke();

        protected override void OnInit() => InitLogic?.Invoke();
    }
}
