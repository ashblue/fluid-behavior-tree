using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class Sequence : CompositeBase {
        protected override TaskStatus OnUpdate () {
            if (AbortType.HasFlag(AbortType.Self)
                && ChildIndex > 0
                && SelfAbortTask != null
                && SelfAbortTask.Update() == TaskStatus.Failure) {
                children[ChildIndex].End();
                Reset();
                return TaskStatus.Failure;
            }

            foreach (var abort in AbortLowerPriorities) {
                if (abort.Update() != TaskStatus.Failure) continue;
                children[ChildIndex].End();
                Reset();
                return TaskStatus.Failure;
            }

            for (var i = ChildIndex; i < children.Count; i++) {
                var child = children[ChildIndex];

                if (child.Enabled) {
                    var status = child.Update();
                    if (status != TaskStatus.Success) {
                        return status;
                    }

                    if (child.IsLowerPriority) {
                        var abortCondition = child.GetAbortCondition();
                        if (abortCondition != null) {
                            AbortLowerPriorities.Add(abortCondition);
                        }
                    }
                }

                ChildIndex++;
            }

            return TaskStatus.Success;
        }
    }
}
