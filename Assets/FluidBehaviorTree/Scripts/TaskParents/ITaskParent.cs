using Adnc.FluidBT.Tasks;
using System.Collections.Generic;

namespace Adnc.FluidBT.TaskParents {
    public interface ITaskParent : ITask {
        List<ITask> children { get; set; }
    }
}