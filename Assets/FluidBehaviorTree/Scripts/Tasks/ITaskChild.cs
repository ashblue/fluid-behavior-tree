using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public interface ITaskChild {
        ITaskUpdate Child { get; set; }
    }
}
