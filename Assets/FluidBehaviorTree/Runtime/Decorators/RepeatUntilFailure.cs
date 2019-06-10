using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class RepeatUntilFailure : DecoratorBase {
        protected override TaskStatus OnUpdate () {
            if (Child.Update() == TaskStatus.Failure) {
                return TaskStatus.Failure;
            }
            
            return TaskStatus.Continue;
        }
    }
}