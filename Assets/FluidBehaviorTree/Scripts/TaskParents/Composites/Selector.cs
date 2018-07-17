using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents.Composites {
    public class Selector : CompositeBase {
        protected override TaskStatus OnUpdate () {
            for (var i = ChildIndex; i < children.Count; i++) {
                var child = children[ChildIndex];

                switch (child.Update()) {
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