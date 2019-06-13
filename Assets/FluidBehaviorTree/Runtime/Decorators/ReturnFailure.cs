using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class ReturnFailure : DecoratorBase {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/Cancel.png";

        protected override TaskStatus OnUpdate () {
            var status = Child.Update();
            if (status == TaskStatus.Continue) {
                return status;
            }

            return TaskStatus.Failure;
        }
    }
}