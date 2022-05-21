using System.Collections.Generic;
using System.IO;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents.Composites
{
    public class Parallel : CompositeBase
    {
        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}CompareArrows.png";

        private readonly Dictionary<ITask, TaskStatus> _childStatus = new();

        protected override TaskStatus OnUpdate()
        {
            var successCount = 0;
            var failureCount = 0;

            foreach (var child in Children)
            {
                var hasChildStatus = _childStatus.TryGetValue(child, out var prevStatus);
                if (hasChildStatus && prevStatus == TaskStatus.Success)
                {
                    successCount++;
                    continue;
                }

                var status = child.Update();
                _childStatus[child] = status;

                if (status == TaskStatus.Failure)
                {
                    failureCount++;
                }
                else if(status == TaskStatus.Success)
                {
                    successCount++;
                }
            }

            if (successCount == Children.Count)
            {
                End();
                return TaskStatus.Success;
            }

            if (failureCount > 0)
            {
                End();
                return TaskStatus.Failure;
            }

            return TaskStatus.Continue;
        }

        public override void Reset()
        {
            _childStatus.Clear();

            base.Reset();
        }

        public override void End()
        {
            foreach (var child in Children)
            {
                child.End();
            }
        }
    }
}
