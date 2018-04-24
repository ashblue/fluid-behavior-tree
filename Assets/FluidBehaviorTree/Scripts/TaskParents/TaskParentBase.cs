using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskParentBase : ITaskParent {
        public bool Enabled { get; set; } = true;

        public List<ITask> children { get; } = new List<ITask>();

        protected virtual int MaxChildren { get; } = -1;
        
        public TaskStatus Update () {
            throw new System.NotImplementedException();
        }

        public ITask Tick () {
            return OnTick();
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