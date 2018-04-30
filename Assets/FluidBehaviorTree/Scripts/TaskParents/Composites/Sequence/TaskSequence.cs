using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class TaskSequence : TaskParentBase {
        private int _childIndex;
        private readonly List<ITask> _abortLowerPriorities = new List<ITask>();

        protected override TaskStatus OnUpdate () {
            if (AbortType.HasFlag(AbortType.Self)
                && _childIndex > 0
                && children[0].Update() == TaskStatus.Failure) {
                children[_childIndex].End();
                Reset();
                return TaskStatus.Failure;
            }

            foreach (var abort in _abortLowerPriorities) {
                if (abort.Update() != TaskStatus.Failure) continue;
                Reset();
                return TaskStatus.Failure;
            }

            for (var i = _childIndex; i < children.Count; i++) {
                var child = children[_childIndex];

                if (child.Enabled) {
                    var status = child.Update();
                    if (status != TaskStatus.Success) {
                        return status;
                    }

                    if (child.IsLowerPriority) {
                        _abortLowerPriorities.Add(child.GetAbortCondition());
                    }
                }

                _childIndex++;
            }

            return TaskStatus.Success;
        }

        public override void Reset (bool hardReset = false) {
            _childIndex = 0;

            base.Reset(hardReset);
        }
    }
}
