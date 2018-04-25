using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public abstract class TaskParentBase : ITaskParent {
        public bool Enabled { get; set; } = true;

        public List<ITask> children { get; } = new List<ITask>();

        protected virtual int MaxChildren { get; } = -1;
        
        public TaskStatus Update () {
            throw new System.NotImplementedException();
        }

        public ITask Tick () {
            return OnTick();
        }

        public virtual void Reset (bool hardReset = false) {
            if (children.Count <= 0) return;
            foreach (var child in children) {
                child.Reset(hardReset);
            }
        }

        protected virtual ITask OnTick () {
            return null;
        }

        public void AddChild (ITask child) {
            if (children.Count < MaxChildren || MaxChildren < 0) {
                children.Add(child);
            }
        }
    }
}