using System;

namespace CleverCrow.Fluid.BTs.Tasks
{
    public class ConditionGeneric : ConditionBase
    {
        public Action exitLogic;
        public Action initLogic;
        public Action startLogic;
        public Func<bool> updateLogic;

        protected override bool OnUpdate()
        {
            if (updateLogic != null) return updateLogic();

            return true;
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
