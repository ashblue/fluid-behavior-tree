using System.IO;
using CleverCrow.Fluid.BTs.Core;
using CleverCrow.Fluid.BTs.Trees.Core.Implementations;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;
using CleverCrow.Fluid.BTs.Trees.Utils;

namespace CleverCrow.Fluid.BTs.Tasks
{
    public abstract class GenericTaskBase
    {
        protected static readonly string PackageRoot = Path.Combine(PathUtils.RootPlaceholder, "/Editor/Icons/Tasks");

        /// <summary>
        /// For custom project icons provide a path from assets to a Texture2D asset. Example `Assets/MyIcon.png`.
        /// Be sure to NOT include the keyword `ROOT` in your path name. This will be replaced with a path
        /// to the package root for Fluid Behavior Tree.
        /// </summary>
        public virtual string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}Play.png";

        public virtual float IconPadding => MathFExtensions.Zero;
        public bool HasBeenActive { get; private set; }

        public IDebugHandler DebugHandler { get; } = new DebugHandler();

        protected ILogWriter LogWriter { get; } = BehaviorTreeConfiguration.DependencyResolver.Resolve<ILogWriter>();

        public virtual TaskStatus Update()
        {
            DebugHandler.OnEventActive();

            HasBeenActive = true;

            return TaskStatus.Success;
        }
    }
}
