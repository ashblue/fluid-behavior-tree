using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public interface ITaskParent : ITask {
        List<ITask> Children { get; }
        ITaskParent AddChild (ITask child);
    }
}