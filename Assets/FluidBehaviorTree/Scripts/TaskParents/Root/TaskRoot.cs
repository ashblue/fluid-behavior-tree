using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskRoot : TaskParentBase {
        protected override int MaxChildren { get; } = 1;

        protected override ITask OnTick () {
            if (children.Count == 0) {
                return null;
            }

            var child = children[0];
            if (!child.Enabled) return null;

            if (child is ITaskParent) {
                return child.Tick();
            }

            var status = child.Update();
            if (status == TaskStatus.Continue) {
                return this;
            }

            return null;
        }
    }
}