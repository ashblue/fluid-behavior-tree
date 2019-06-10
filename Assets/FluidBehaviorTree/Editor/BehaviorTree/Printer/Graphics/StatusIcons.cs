using System;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class StatusIcons {
        private const string ICON_STATUS_PATH = "ROOT/Editor/Icons/Status";

        private TextureLoader Success { get; } = new TextureLoader($"{ICON_STATUS_PATH}/Success.png");
        private TextureLoader Failure { get; } = new TextureLoader($"{ICON_STATUS_PATH}/Failure.png");
        private TextureLoader Continue { get; } = new TextureLoader($"{ICON_STATUS_PATH}/Continue.png");

        public TextureLoader GetIcon (TaskStatus status) {
            switch (status) {
                case TaskStatus.Success:
                    return Success;
                case TaskStatus.Failure:
                    return Failure;
                case TaskStatus.Continue:
                    return Continue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}
