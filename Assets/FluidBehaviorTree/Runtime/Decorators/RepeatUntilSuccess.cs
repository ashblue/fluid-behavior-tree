using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class RepeatUntilSuccess : DecoratorBase {
        protected override TaskStatus OnUpdate () {
            if (Child == null) {
                return TaskStatus.Success;
            }

            var childStatus = Child.Update();
            var status = childStatus;

            if (status == TaskStatus.Success) {
                return TaskStatus.Success;
            }
            else {
                return TaskStatus.Continue;
            }
        }
    }
}