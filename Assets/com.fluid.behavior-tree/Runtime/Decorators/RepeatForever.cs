using System.IO;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators
{
    public class RepeatForever : DecoratorBase
    {
        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}Repeat.png";

        protected override TaskStatus OnUpdate()
        {
            Child.Update();
            return TaskStatus.Continue;
        }
    }
}
