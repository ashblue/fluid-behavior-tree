using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public class Parallel : CompositeBase {
        private readonly Dictionary<ITask, TaskStatus> _childStatus = new Dictionary<ITask, TaskStatus>();

        protected override TaskStatus OnUpdate () {
            var successCount = 0;
            var failureCount = 0;

            foreach (var child in children) {
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

            if (successCount == children.Count) {
                foreach (var child in children) {
                    child.End();
                }

                return TaskStatus.Success;
            }

            if (failureCount > 0) {
                foreach (var child in children) {
                    child.End();
                }

                return TaskStatus.Failure;
            }

            return TaskStatus.Continue;
        }
    }
}