using System;

namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    public class ActionGeneric : ActionBase
    {
        public Action ExitLogic { get; set; }
        public Action InitLogic { get; set; }
        public Action StartLogic { get; set; }
        public Func<TaskStatus> UpdateLogic { get; set; }

        protected override TaskStatus OnUpdate() => UpdateLogic?.Invoke() ?? TaskStatus.Success;

        protected override void OnStart() => StartLogic?.Invoke();

        protected override void OnExit() => ExitLogic?.Invoke();

        protected override void OnInit() => InitLogic?.Invoke();
    }
}
