using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Trees
{
    public interface IBehaviorTree
    {
        string Name { get; }
        TaskRoot Root { get; }
        int TickCount { get; }

        void AddActiveTask(ITask task);
        void RemoveActiveTask(ITask task);
    }
}
