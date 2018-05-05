using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class Selector : CompositeBase {
        List<int> _lowerPriorityIndices = new List<int>();

        protected override TaskStatus OnUpdate () {
            // @NOTE Consider abort self being able to revaluate any immediate child condition that has already been run
            if (AbortSelf(TaskStatus.Success)) {
                return Update();
            }

            foreach (var abort in AbortLowerPriorities) {
                if (abort.GetAbortStatus() != TaskStatus.Success) continue;
                children[ChildIndex].End();

                var abortIndex = AbortLowerPriorities.IndexOf(abort);
                var restoreIndex = _lowerPriorityIndices[abortIndex];
                var restoreAborts = AbortLowerPriorities.GetRange(0, abortIndex);
                var restoreAbortIndices = _lowerPriorityIndices.GetRange(0, abortIndex);

                Reset();

                ChildIndex = restoreIndex;
                AbortLowerPriorities = restoreAborts;
                _lowerPriorityIndices = restoreAbortIndices;

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
                        AbortLowerPriorities.Add(abortCondition);
                        _lowerPriorityIndices.Add(i);
                    }
                }

                ChildIndex++;
            }

            return TaskStatus.Failure;
        }

        public override ITask GetAbortCondition () {
            return this;
        }

        public override void Reset (bool hardReset = false) {
            _lowerPriorityIndices.Clear();

            base.Reset(hardReset);
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