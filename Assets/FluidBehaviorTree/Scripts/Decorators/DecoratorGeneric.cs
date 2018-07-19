using System;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.Decorators {
    public class DecoratorGeneric : DecoratorBase {
        public Func<DecoratorBase, TaskStatus> updateLogic;

        protected override TaskStatus OnUpdate () {
            if (updateLogic != null) {
                return updateLogic(this);
            }

            return child.Update();
        }
    }
}