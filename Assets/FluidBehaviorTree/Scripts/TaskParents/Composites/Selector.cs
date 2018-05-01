using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class Selector : CompositeBase {
        protected override TaskStatus OnUpdate () {
            if (AbortType.HasFlag(AbortType.Self)
                && ChildIndex > 0
                && SelfAbortTask != null
                && SelfAbortTask.Update() == TaskStatus.Success) {
                children[ChildIndex].End();
                Reset();
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

                ChildIndex++;
            }

            return TaskStatus.Failure;
        }
    }
}