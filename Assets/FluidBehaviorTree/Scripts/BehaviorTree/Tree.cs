using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using UnityEngine;

namespace Adnc.FluidBT.Trees {
    public class Tree {
        private readonly GameObject _owner;
        private TaskStatus _lastStatus;
        private int _tickCount;
        
        public TaskRoot Root { get; } = new TaskRoot();

        public Tree (GameObject owner) {
            _owner = owner;
            Root.Owner = owner;
        }

        public void AddNode (ITaskParent parent, ITask child) {
            parent.AddChild(child);
            child.Owner = _owner;
        }

        public void Tick () {
            _lastStatus = Root.Update();
            _tickCount++;
            
            // Possible way to automate triggering reset
//            _lastStatus = Root.Update(_tickCount != 0 && _lastStatus != TaskStatus.Continue);
        }
    }
}