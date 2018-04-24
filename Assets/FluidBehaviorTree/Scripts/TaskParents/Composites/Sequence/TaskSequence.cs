using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskSequence : ITaskParent {
        public bool Enabled { get; set; } = true;
        public List<ITask> children { get; set; } = new List<ITask>();

        public TaskStatus Update () {
            return TaskStatus.Success;
        }

        public ITask Tick () {
            throw new System.NotImplementedException();
        }
    }
}
