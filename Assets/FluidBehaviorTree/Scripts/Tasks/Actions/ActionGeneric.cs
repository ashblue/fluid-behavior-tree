using System;

namespace Adnc.FluidBT.Tasks.Actions {
    public class ActionGeneric : ActionBase {
        public Func<TaskStatus> updateLogic;
        public Action startLogic;
        public Action initLogic;
        public Action exitLogic;

        protected override TaskStatus OnUpdate () {
            if (updateLogic != null) {
                return updateLogic();
            }

            return TaskStatus.Success;
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