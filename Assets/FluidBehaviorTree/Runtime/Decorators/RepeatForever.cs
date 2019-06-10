using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class RepeatForever : DecoratorBase {
        protected override TaskStatus OnUpdate () {
            Child.Update();
            return TaskStatus.Continue;
        }
    }
}