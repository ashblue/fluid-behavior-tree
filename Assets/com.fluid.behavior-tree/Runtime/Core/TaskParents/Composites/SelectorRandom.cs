using System.IO;
using CleverCrow.Fluid.BTs.com.fluid.behavior_tree.Runtime.Core;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;

namespace CleverCrow.Fluid.BTs.TaskParents.Composites
{
    /// <summary>
    ///     Randomly selects a child node with a shuffle algorithm
    /// </summary>
    public class SelectorRandom : CompositeBase
    {
        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}LinearScale.png";

        protected IRandomHandler RandomHandler { get; } = BehaviorTreeConfiguration.DependencyResolver.Resolve<IRandomHandler>();

        private bool _init;

        protected override TaskStatus OnUpdate()
        {
            if (!_init)
            {
                ShuffleChildren();
                _init = true;
            }

            for (var i = ChildIndex; i < Children.Count; i++)
            {
                var child = Children[ChildIndex];

                switch (child.Update())
                {
                    case TaskStatus.Success:
                        return TaskStatus.Success;
                    case TaskStatus.Continue:
                        return TaskStatus.Continue;
                }

                ChildIndex++;
            }

            return TaskStatus.Failure;
        }

        public override void Reset()
        {
            base.Reset();

            ShuffleChildren();
        }

        private void ShuffleChildren()
        {
            var childrenCount = Children.Count;
            while (childrenCount > 1)
            {
                childrenCount--;
                var random = RandomHandler.NextInt(childrenCount + 1);
                (Children[random], Children[childrenCount]) = (Children[childrenCount], Children[random]);
            }
        }
    }
}
