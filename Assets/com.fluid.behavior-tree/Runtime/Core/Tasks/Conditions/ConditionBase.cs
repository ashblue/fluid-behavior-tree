using System.IO;

namespace CleverCrow.Fluid.BTs.Tasks
{
    public abstract class ConditionBase : TaskBase
    {
        private const float DefaultIconPadding = 10f;

        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}Question.png";
        public override float IconPadding => DefaultIconPadding;

        protected override TaskStatus GetUpdate() => OnUpdate() ? TaskStatus.Success : TaskStatus.Failure;
        protected abstract bool OnUpdate();

    }
}
