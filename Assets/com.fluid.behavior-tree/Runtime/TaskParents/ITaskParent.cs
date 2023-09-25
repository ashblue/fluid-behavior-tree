using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents {
    public class ITaskParent : GenericTaskBase {
        public virtual ITaskParent AddChild (ITask child) { return null; }
    }
}
