using System;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators
{
    public class DecoratorGeneric : DecoratorBase
    {
        public Func<ITask, TaskStatus> UpdateLogic;

        protected override TaskStatus OnUpdate() => UpdateLogic?.Invoke(Child) ?? Child.Update();
    }
}
