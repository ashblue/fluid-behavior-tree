using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class Selector : CompositeBase {
        protected override TaskStatus OnUpdate () {
            if (AbortSelf(TaskStatus.Success)) {
                return Update();
            }
            
            foreach (var abort in AbortLowerPriorities) {
                if (abort.GetAbortStatus() != TaskStatus.Success) continue;
                children[ChildIndex].End();
                Reset();
                // @TODO Should use the abort's index to figure out what to revaluate (should not run entire branch)
                return Update();
            }
            
            for (var i = ChildIndex; i < children.Count; i++) {
                var child = children[ChildIndex];

                switch (child.Update()) {
                    case TaskStatus.Success:
                        return TaskStatus.Success;
                    case TaskStatus.Continue:
                        return TaskStatus.Continue;
                }
                
                if (child.IsLowerPriority) {
                    var abortCondition = child.GetAbortCondition();
                    if (abortCondition != null) {
                        // @TODO Abort must include the current child index
                        AbortLowerPriorities.Add(abortCondition);
                    }
                }

                ChildIndex++;
            }

            return TaskStatus.Failure;
        }

        public override ITask GetAbortCondition () {
            return this;
        }

        // @TODO Cache the abort children on first run since they will never change
        public override TaskStatus GetAbortStatus () {
            foreach (var child in children) {
                var abort = child.GetAbortCondition();
                if (abort == null) continue;

                if (abort.GetAbortStatus() == TaskStatus.Success) {
                    return TaskStatus.Success;
                }
            }

            return TaskStatus.Failure;
        }
    }
}