using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents.Composites {
    public class Parallel : CompositeBase {
        private readonly Dictionary<ITask, TaskStatus> _childStatus = new Dictionary<ITask, TaskStatus>();

        public override string IconPath { get; } = $"{PACKAGE_ROOT}/CompareArrows.png";

        protected override TaskStatus OnUpdate () {
            var successCount = 0;
            var failureCount = 0;

            foreach (var child in Children) {
                TaskStatus prevStatus;
                if (_childStatus.TryGetValue(child, out prevStatus) && prevStatus == TaskStatus.Success) {
                    successCount++;
                    continue;
                }

                var status = child.Update();
                _childStatus[child] = status;

                switch (status) {
                    case TaskStatus.Failure:
                        failureCount++;
                        break;
                    case TaskStatus.Success:
                        successCount++;
                        break;
                }
            }

            if (successCount == Children.Count) {
                End();
                return TaskStatus.Success;
            }

            if (failureCount > 0) {
                End();
                return TaskStatus.Failure;
            }

            return TaskStatus.Continue;
        }

        public override void ResetTask () {
            _childStatus.Clear();

            base.ResetTask();
        }

        public override void End () {
            foreach (var child in Children) {
                child.End();
            }
        }
    }
}
