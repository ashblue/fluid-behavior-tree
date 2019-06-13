using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class ReturnSuccess : DecoratorBase {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/Checkmark.png";

        protected override TaskStatus OnUpdate () {
            var status = Child.Update();
            if (status == TaskStatus.Continue) {
                return status;
            }

            return TaskStatus.Success;
        }
    }
}