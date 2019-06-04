using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class RepeatUntilFailure : DecoratorBase {
        protected override TaskStatus OnUpdate () {
            if (Child == null) {
                return TaskStatus.Failure;
            }

            var childStatus = Child.Update();
            var status = childStatus;

            if (status == TaskStatus.Failure) {
                return TaskStatus.Failure;
            }
            else {
                return TaskStatus.Continue;
            }
        }
    }
}