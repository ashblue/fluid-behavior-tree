using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class Selector : CompositeBase {
        protected override TaskStatus OnUpdate () {
            if (AbortSelf(TaskStatus.Success)) {
                return Update();
            }
            
            foreach (var abort in AbortLowerPriorities) {
                if (abort.Update() != TaskStatus.Success) continue;
                children[ChildIndex].End();
                Reset();
                // @TODO Should use the abort's index to figure out what to revaluate
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
    }
}