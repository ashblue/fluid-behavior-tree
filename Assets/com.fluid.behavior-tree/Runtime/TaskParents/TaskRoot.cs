using System.IO;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents
{
    public class TaskRoot : TaskParentBase
    {
        private const string DefaultName = "Root";

        public override string Name { get; set; } = DefaultName;
        protected override int MaxChildren => 1;
        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}DownArrow.png";

        protected override TaskStatus OnUpdate()
        {
            if (Children.Count == 0)
            {
                return TaskStatus.Success;
            }

            var child = Children[0];
            return child.Update();
        }

        public override void End()
        {
        }
    }
}
