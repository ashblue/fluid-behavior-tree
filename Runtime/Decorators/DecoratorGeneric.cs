using System;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators {
    public class DecoratorGeneric : DecoratorBase {
        public Func<ITask, TaskStatus> updateLogic;

        protected override TaskStatus OnUpdate () {
            if (updateLogic != null) {
                return updateLogic(Child);
            }

            return Child.Update();
        }
    }
}