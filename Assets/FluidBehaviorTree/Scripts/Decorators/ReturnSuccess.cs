using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.Decorators {
    public class ReturnSuccess : DecoratorBase {
        protected override TaskStatus OnUpdate () {
            var status = Child.Update();
            if (status == TaskStatus.Continue) {
                return status;
            }

            return TaskStatus.Success;
        }
    }
}