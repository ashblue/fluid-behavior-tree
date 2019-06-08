using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class Inverter : DecoratorBase {
        public override string IconPath { get; } = $"{_iconPath}/Invert.png";

        protected override TaskStatus OnUpdate () {
            if (Child == null) {
                return TaskStatus.Success;
            }

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