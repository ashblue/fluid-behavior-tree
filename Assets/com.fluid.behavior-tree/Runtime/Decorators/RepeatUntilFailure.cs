using System.IO;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators
{
    public class RepeatUntilFailure : DecoratorBase
    {
        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}EventBusy.png";

        protected override TaskStatus OnUpdate()
        {
            return Child.Update() == TaskStatus.Failure
                ? TaskStatus.Failure
                : TaskStatus.Continue;
        }
    }
}
