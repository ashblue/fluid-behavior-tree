using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.Decorators {
    public class Inverter : ITask {
        private bool _enabled = true;
        public bool Enabled {
            get { return child != null && _enabled; }
            set { _enabled = value; }
        }

        public bool IsLowerPriority { get; } = false;
        public TaskStatus LastStatus { get; private set; }
        public ITask child;

        public TaskStatus Update () {
            if (child == null) {
                return TaskStatus.Success;
            }

            var childStatus = child.Update();
            var status = childStatus;

            switch (childStatus) {
                case TaskStatus.Success:
                    status = TaskStatus.Failure;
                    break;
                case TaskStatus.Failure:
                    status = TaskStatus.Success;
                    break;
            }

            // @TODO Can be moved to base class
            LastStatus = status;

            return status;
        }

        public ITask GetAbortCondition () {
            if (child.GetAbortCondition() != null) {
                return this;
            }

            return null;
        }

        public void End () {
            child.End();
        }

        public void Reset (bool hardReset = false) {
            child.Reset();
        }

        public TaskStatus GetAbortStatus () {
            return Update();
        }
    }
}