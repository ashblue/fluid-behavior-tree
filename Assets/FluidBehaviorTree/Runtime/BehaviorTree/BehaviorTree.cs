using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees {
    public class BehaviorTree {
        private readonly GameObject _owner;
        
        public int TickCount { get; private set; }
        
        public TaskRoot Root { get; } = new TaskRoot();

        public BehaviorTree (GameObject owner) {
            _owner = owner;
            SyncNodes(Root);
        }
        
        public TaskStatus Tick () {
            var status = Root.Update();
            if (status != TaskStatus.Continue) {
                TickCount++;
            }

            return status;
        }

        public void Reset () {
            TickCount++;
        }

        public void AddNode (ITaskParent parent, ITask child) {
            parent.AddChild(child);
            child.ParentTree = this;
            child.Owner = _owner;
        }

        public void Splice (ITaskParent parent, BehaviorTree tree) {
            parent.AddChild(tree.Root);

            SyncNodes(tree.Root);
        }

        private void SyncNodes (ITaskParent taskParent) {
            taskParent.Owner = _owner;
            taskParent.ParentTree = this;
            
            foreach (var child in taskParent.Children) {
                child.Owner = _owner;
                child.ParentTree = this;

                var parent = child as ITaskParent;
                if (parent != null) {
                    SyncNodes(parent);
                }
            }
        }
    }
}