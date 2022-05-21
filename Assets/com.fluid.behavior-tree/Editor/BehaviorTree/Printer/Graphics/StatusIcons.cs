using System;
using System.IO;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees.Utils;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class StatusIcons
    {
        // TODO: Change the way path is used to support any platform
        private const string IconStatusPath = PathUtils.RootPlaceholder + "/Editor/Icons/Status";

        private TextureLoader Success { get; } = new($"{IconStatusPath}{Path.DirectorySeparatorChar}Success.png");
        private TextureLoader Failure { get; } = new($"{IconStatusPath}{Path.DirectorySeparatorChar}Failure.png");
        private TextureLoader Continue { get; } = new($"{IconStatusPath}{Path.DirectorySeparatorChar}Continue.png");

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
