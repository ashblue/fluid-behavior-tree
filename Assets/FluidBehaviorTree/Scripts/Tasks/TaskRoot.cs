using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskRoot : ITaskRoot {
        public ITaskUpdate Child { get; set; }

        public bool Enabled { get; set; } = true;

        public TaskStatus Update () {
            if (Child != null && Child.Enabled) {
                return Child.Update();
            }

            return TaskStatus.Failure;
        }
    }
}