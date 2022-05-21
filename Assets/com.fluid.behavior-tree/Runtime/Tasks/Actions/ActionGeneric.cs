using System;

namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    public class ActionGeneric : ActionBase
    {
        // TODO: Convert into properties
        public Action ExitLogic;
        public Action InitLogic;
        public Action StartLogic;
        public Func<TaskStatus> UpdateLogic;

        protected override TaskStatus OnUpdate() => UpdateLogic?.Invoke() ?? TaskStatus.Success;

        protected override void OnStart() => StartLogic?.Invoke();

        protected override void OnExit() => ExitLogic?.Invoke();

        protected override void OnInit() => InitLogic?.Invoke();
    }
}
