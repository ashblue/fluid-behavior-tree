using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents.Composites {
    public class Parallel : CompositeBase {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/CompareArrows.png";

        protected override TaskStatus OnUpdate () {
            TaskStatus status = TaskStatus.Success;
            foreach(var child in Children)
                status |= child.Update();
            if(TaskStatus.Failure == (status&TaskStatus.Failure))
                return TaskStatus.Failure;
            else if(TaskStatus.Continue == (status&TaskStatus.Continue))
                return TaskStatus.Continue;
            return TaskStatus.Success;
        }

        public override void Reset () {
            base.Reset();
        }

        public override void End () {
            foreach (var child in Children) {
                child.End();
            }
        }
    }
}
