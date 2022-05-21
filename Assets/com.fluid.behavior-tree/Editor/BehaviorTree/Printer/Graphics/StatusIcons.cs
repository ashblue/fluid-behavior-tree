using System;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class StatusIcons
    {
        // TODO: Change the way path is used to support any platform
        private const string IconStatusPath = "ROOT/Editor/Icons/Status";

        private TextureLoader Success { get; } = new($"{IconStatusPath}/Success.png");
        private TextureLoader Failure { get; } = new($"{IconStatusPath}/Failure.png");
        private TextureLoader Continue { get; } = new($"{IconStatusPath}/Continue.png");

        public TextureLoader GetIcon(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.Success => Success,
                TaskStatus.Failure => Failure,
                TaskStatus.Continue => Continue,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
    }
}
