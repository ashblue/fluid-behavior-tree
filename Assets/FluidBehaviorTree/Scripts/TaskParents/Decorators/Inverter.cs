using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.Decorators {
    public class Inverter : ITask {
        private bool _enabled = true;
        public bool Enabled {
            get { return child != null && _enabled; }
            set { _enabled = value; }
        }

        public bool IsLowerPriority { get; } = false;
        public TaskStatus LastStatus { get; }
        public ITask child;

        public TaskStatus Update () {
            throw new System.NotImplementedException();
        }

        public ITask GetAbortCondition () {
            throw new System.NotImplementedException();
        }

        public void End () {
            throw new System.NotImplementedException();
        }

        public void Reset (bool hardReset = false) {
            throw new System.NotImplementedException();
        }

        public TaskStatus GetAbortStatus () {
            throw new System.NotImplementedException();
        }
    }
}