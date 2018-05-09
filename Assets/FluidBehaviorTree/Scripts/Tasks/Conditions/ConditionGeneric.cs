using System;

namespace Adnc.FluidBT.Tasks {
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
            initLogic?.Invoke();
        }

        protected override void OnInit () {
            exitLogic?.Invoke();
        }
    }
}