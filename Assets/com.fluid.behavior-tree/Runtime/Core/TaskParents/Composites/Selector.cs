﻿using System.IO;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.TaskParents.Composites
{
    public class Selector : CompositeBase
    {
        public override string IconPath => $"{PackageRoot}{Path.DirectorySeparatorChar}LinearScale.png";

        protected override TaskStatus OnUpdate()
        {
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
    }
}
