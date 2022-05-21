using System.IO;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators
{
    public class RepeatUntilSuccess : DecoratorBase
    {
        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}EventAvailable.png";

        protected override TaskStatus OnUpdate()
        {
            return Child.Update() == TaskStatus.Success
                ? TaskStatus.Success
                : TaskStatus.Continue;
        }
    }
}
