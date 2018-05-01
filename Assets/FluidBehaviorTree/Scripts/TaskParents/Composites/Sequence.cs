using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class Sequence : TaskParentBase {
        private ITask _selfAbortTask;
        private int _childIndex;
        private readonly List<ITask> _abortLowerPriorities = new List<ITask>();

        protected override TaskStatus OnUpdate () {
            if (AbortType.HasFlag(AbortType.Self)
                && _childIndex > 0
                && _selfAbortTask != null
                && _selfAbortTask.Update() == TaskStatus.Failure) {
                children[_childIndex].End();
                Reset();
                return TaskStatus.Failure;
            }

            foreach (var abort in _abortLowerPriorities) {
                if (abort.Update() != TaskStatus.Failure) continue;
                children[_childIndex].End();
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
                        var abortCondition = child.GetAbortCondition();
                        if (abortCondition != null) {
                            _abortLowerPriorities.Add(abortCondition);
                        }
                    }
                }

                _childIndex++;
            }

            return TaskStatus.Success;
        }
        
        public override ITaskParent AddChild (ITask child) {
            if (children.Count == 0) {
                _selfAbortTask = child.GetAbortCondition();
            }
            
            return base.AddChild(child);
        }

        public override void End () {
            children[_childIndex].End();
        }

        public override void Reset (bool hardReset = false) {
            _childIndex = 0;
            _abortLowerPriorities.Clear();

            base.Reset(hardReset);
        }
    }
}
