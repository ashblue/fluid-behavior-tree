using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskRoot : TaskParentBase {
        protected override ITask OnTick () {
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