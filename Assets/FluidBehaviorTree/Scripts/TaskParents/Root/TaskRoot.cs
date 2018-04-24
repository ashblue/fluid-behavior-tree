using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskRoot : ITaskParent {
        public List<ITask> children { get; set; } = new List<ITask>();

        public bool Enabled { get; set; } = true;

        public TaskStatus Update () {
            throw new System.NotImplementedException();
        }

        public ITask Tick () {
            if (children.Count == 0) {
                return null;
            }

            var child = children[0];
            if (child.Enabled) {
                child.Update();
            }

            return null;
        }

    }
}