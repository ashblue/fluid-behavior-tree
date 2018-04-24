using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskParentBase : ITaskParent {
        protected virtual int MaxChildren { get; } = -1;
        
        public List<ITask> children { get; set; }

        public bool Enabled { get; set; }
        
        public TaskStatus Update () {
            throw new System.NotImplementedException();
        }

        public ITask Tick () {
            throw new System.NotImplementedException();
        }

    }
}