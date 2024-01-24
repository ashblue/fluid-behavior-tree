using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents.Composites {
    public class Sequence : CompositeBase {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/RightArrow.png";

        protected override TaskStatus OnUpdate () {            
            for (var i = ChildIndex; i < Children.Count; i++) {
                var child = Children[ChildIndex];

                var status = child.Update();
                if (status != TaskStatus.Success) {
                    return status;
                }

                ChildIndex++;
            }

            return TaskStatus.Success;
        }
    }
}
