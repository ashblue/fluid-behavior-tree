using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using UnityEngine;

namespace Adnc.FluidBT.Trees {
    public class Tree {
        private readonly GameObject _owner;
        
        public int TickCount { get; private set; }
        
        public TaskRoot Root { get; } = new TaskRoot();

        public Tree (GameObject owner) {
            _owner = owner;
            Root.ParentTree = this;
            Root.Owner = owner;
        }

        public void Reset () {
            TickCount++;
        }

        public void AddNode (ITaskParent parent, ITask child) {
            parent.AddChild(child);
            child.ParentTree = this;
            child.Owner = _owner;
        }

        public void Tick () {
            if (Root.Update() != TaskStatus.Continue) {
                TickCount++;
            }
        }
    }
}