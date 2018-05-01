using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public abstract class CompositeBase : TaskParentBase {
        public ITask SelfAbortTask { get; protected set; }
        public int ChildIndex { get; protected set; }
        public List<ITask> AbortLowerPriorities { get; } = new List<ITask>();

        public override ITaskParent AddChild (ITask child) {
            if (children.Count == 0) {
                SelfAbortTask = child.GetAbortCondition();
            }
            
            return base.AddChild(child);
        }

        public override void End () {
            if (ChildIndex < children.Count) {
                children[ChildIndex].End();
            }
        }
        
        public override void Reset (bool hardReset = false) {
            ChildIndex = 0;
            AbortLowerPriorities.Clear();
            SelfAbortTask = null;

            base.Reset(hardReset);
        }
    }
}