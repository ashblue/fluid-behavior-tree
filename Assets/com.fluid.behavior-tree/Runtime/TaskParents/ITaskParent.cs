using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents {
    public abstract class ITaskParent : GenericTaskBase {
        public abstract ITaskParent AddChild (ITask child);
    }
}
