using System;

namespace CleverCrow.Fluid.BTs.Tasks
{
    public class ConditionGeneric : ConditionBase
    {
        // TODO: Convert into properties
        public Action ExitLogic;
        public Action InitLogic;
        public Action StartLogic;
        public Func<bool> UpdateLogic;

        protected override bool OnUpdate() => UpdateLogic == null || UpdateLogic();

        protected override void OnStart() => StartLogic?.Invoke();

        protected override void OnExit() => ExitLogic?.Invoke();

        protected override void OnInit() => InitLogic?.Invoke();
    }
}
