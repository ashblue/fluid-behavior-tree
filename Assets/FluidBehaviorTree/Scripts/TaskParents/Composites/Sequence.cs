using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class Sequence : CompositeBase {
        protected override TaskStatus OnUpdate () {
            if (AbortSelf(TaskStatus.Failure)) {
                return TaskStatus.Failure;
            }

            foreach (var abort in AbortLowerPriorities) {
                if (abort.GetAbortStatus() != TaskStatus.Failure) continue;
                children[ChildIndex].End();
                Reset();
                return TaskStatus.Failure;
            }

            for (var i = ChildIndex; i < children.Count; i++) {
                var child = children[ChildIndex];

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

                ChildIndex++;
            }

            return TaskStatus.Success;
        }

        public override ITask GetAbortCondition () {
            if (children.Count > 0) {
                return children[0].GetAbortCondition();
            }

            return null;
        }
    }
}
