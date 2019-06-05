using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents {
    public interface ITaskParent : ITask {
        ITaskParent AddChild (ITask child);
    }
}