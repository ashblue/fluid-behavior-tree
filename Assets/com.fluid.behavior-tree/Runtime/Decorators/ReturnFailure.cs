using System.IO;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators
{
    public class ReturnFailure : DecoratorBase
    {
        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}Cancel.png";

        protected override TaskStatus OnUpdate()
        {
            var status = Child.Update();
            return status == TaskStatus.Continue ? status : TaskStatus.Failure;
        }
    }
}
