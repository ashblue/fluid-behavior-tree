using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents {
    [System.Serializable]
    public class TaskRoot : TaskParentBase {
        public override string Name { get; set; } = "Root";
        protected override int MaxChildren { get; } = 1;
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/DownArrow.png";

        protected override TaskStatus OnUpdate () {
            if (Children.Count == 0) {
                return TaskStatus.Success;
            }

            var child = Children[0];
            return child.Update();
        }

        public override void End () {
        }
    }
}
