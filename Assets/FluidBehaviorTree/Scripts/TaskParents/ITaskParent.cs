using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public interface ITaskParent : ITask {
        List<ITask> children { get; }
        ITaskParent AddChild (ITask child);
    }
}