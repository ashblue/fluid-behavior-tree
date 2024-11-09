using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class Inverter : DecoratorBase {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/Invert.png";

        protected override TaskStatus OnUpdate () {
            var childStatus = Child.Update();
            var status = childStatus;

            switch (childStatus) {
                case TaskStatus.Success:
                    status = TaskStatus.Failure;
                    break;
                case TaskStatus.Failure:
                    status = TaskStatus.Success;
                    break;
            }

            return status;
        }
    }
}