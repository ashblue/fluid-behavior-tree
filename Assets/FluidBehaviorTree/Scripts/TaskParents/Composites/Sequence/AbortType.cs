using System;

namespace Adnc.FluidBT.TaskParents {
    [Flags]
    public enum AbortType {
        None = 0,
        Self = 1,
        LowerPriority = 2
    }
}