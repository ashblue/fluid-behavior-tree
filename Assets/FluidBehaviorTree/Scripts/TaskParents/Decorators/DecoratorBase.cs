using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Trees;

namespace Adnc.FluidBT.Decorators {
    public abstract class DecoratorBase : ITask {
        public ITask child;

        private bool _enabled = true;
        public bool Enabled {
            get { return child != null && child.Enabled && _enabled; }
            set { _enabled = value; }
        }

        public bool IsLowerPriority { get; } = false;
        public BehaviorTree Owner { get; set; }
        public TaskStatus LastStatus { get; private set; }

        public TaskStatus Update () {
            var status = OnUpdate();
            LastStatus = status;

            return status;
        }

        protected abstract TaskStatus OnUpdate ();

        public ITask GetAbortCondition () {
            if (child.GetAbortCondition() != null) {
                return this;
            }

            return null;
        }

        public TaskStatus GetAbortStatus () {
            return Update();
        }

        public void End () {
            child.End();
        }

        public void Reset (bool hardReset = false) {
            child.Reset();
        }
    }
}