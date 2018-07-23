using System;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.Decorators {
    public class DecoratorGeneric : DecoratorBase {
        public Func<ITask, TaskStatus> updateLogic;

        protected override TaskStatus OnUpdate () {
            if (Child == null) {
                return TaskStatus.Success;
            }
            
            if (updateLogic != null) {
                return updateLogic(Child);
            }

            return Child.Update();
        }
    }
}