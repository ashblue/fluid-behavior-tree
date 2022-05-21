using System;

namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    public class ActionGeneric : ActionBase
    {
        public Action exitLogic;
        public Action initLogic;
        public Action startLogic;
        public Func<TaskStatus> updateLogic;

        protected override TaskStatus OnUpdate()
        {
            if (updateLogic != null) return updateLogic();

            return TaskStatus.Success;
        }

        protected override void OnStart()
        {
            startLogic?.Invoke();
        }

        protected override void OnExit()
        {
            exitLogic?.Invoke();
        }

        protected override void OnInit()
        {
            initLogic?.Invoke();
        }
    }
}
