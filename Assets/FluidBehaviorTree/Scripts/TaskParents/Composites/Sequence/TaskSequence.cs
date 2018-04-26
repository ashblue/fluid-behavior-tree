using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskSequence : TaskParentBase {
        protected override TaskStatus OnUpdate () {
            foreach (var child in children) {
                var status = child.Update();
                if (status != TaskStatus.Success) {
                    return status;
                }
            }

            return TaskStatus.Success;
        }
    }
}
