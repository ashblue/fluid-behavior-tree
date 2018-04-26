using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskRoot : TaskParentBase {
        protected override int MaxChildren { get; } = 1;

        protected override TaskStatus OnUpdate () {
            if (children.Count == 0) {
                return TaskStatus.Success;
            }

            var child = children[0];
            if (!child.Enabled) {
                return TaskStatus.Success;
            }

            return child.Update();
        }
    }
}