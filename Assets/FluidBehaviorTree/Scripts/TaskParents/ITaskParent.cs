using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public interface ITaskParent : ITask {
        List<ITask> children { get; }
        void AddChild (ITask child);
    }
}