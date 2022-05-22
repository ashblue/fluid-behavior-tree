using System.IO;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Decorators
{
    public class Inverter : DecoratorBase
    {
        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}Invert.png";

        protected override TaskStatus OnUpdate()
        {
            var childStatus = Child.Update();

            return childStatus switch
            {
                TaskStatus.Success => TaskStatus.Failure,
                TaskStatus.Failure => TaskStatus.Success,
                _ => childStatus
            };
        }
    }
}
