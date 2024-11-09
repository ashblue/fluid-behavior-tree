using System;

namespace CleverCrow.Fluid.BTs.Tasks {
    public class ConditionGeneric : ConditionBase {
        public Func<bool> updateLogic;
        public Action startLogic;
        public Action initLogic;
        public Action exitLogic;

        protected override bool OnUpdate () {
            if (updateLogic != null) {
                return updateLogic();
            }

            return true;
        }

        protected override void OnStart () {
            startLogic?.Invoke();
        }

        protected override void OnExit () {
            exitLogic?.Invoke();
        }

        protected override void OnInit () {
            initLogic?.Invoke();
        }
    }
}