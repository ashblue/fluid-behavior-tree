using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class RepeatUntilSuccess : DecoratorBase {
        protected override TaskStatus OnUpdate () {
            if (Child.Update() == TaskStatus.Success) {
                return TaskStatus.Success;
            }

            return TaskStatus.Continue;
        }
    }
}