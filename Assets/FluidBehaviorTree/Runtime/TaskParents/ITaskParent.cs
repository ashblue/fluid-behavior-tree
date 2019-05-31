using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents {
    public interface ITaskParent : ITask {
        List<ITask> Children { get; }
        ITaskParent AddChild (ITask child);
    }
}