using Adnc.FluidBT.Tasks;
using UnityEngine;

namespace Adnc.FluidBT.Decorators {
    public abstract class DecoratorBase : ITask {
        public ITask child;

        private bool _enabled = true;
        public bool Enabled {
            get { return child != null && child.Enabled && _enabled; }
            set { _enabled = value; }
        }

        public GameObject Owner { get; set; }
        public TaskStatus LastStatus { get; private set; }

        public TaskStatus Update () {
            var status = OnUpdate();
            LastStatus = status;

            return status;
        }

        protected abstract TaskStatus OnUpdate ();

        public void End () {
            child.End();
        }

        public void Reset (bool hardReset = false) {
            child.Reset();
        }
    }
}