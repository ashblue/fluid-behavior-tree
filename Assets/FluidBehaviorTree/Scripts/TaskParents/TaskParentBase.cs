using System.Collections.Generic;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Trees;

namespace Adnc.FluidBT.TaskParents {
    public abstract class TaskParentBase : ITaskParent {
        private bool _enabled = true;
        
        public AbortType AbortType { get; set; } = AbortType.None;
        public TaskStatus LastStatus { get; private set; }

        public bool Enabled {
            get { return children.Count != 0 && _enabled; }
            set { _enabled = value; }
        }

        public List<ITask> children { get; } = new List<ITask>();

        protected virtual int MaxChildren { get; } = -1;

        public bool IsLowerPriority => AbortType.HasFlag(AbortType.LowerPriority);
        public BehaviorTree Owner { get; set; }
        public bool ValidAbortCondition { get; } = false;

        public TaskStatus Update () {
            var status = OnUpdate();
            LastStatus = status;

            return status;
        }

        public virtual ITask GetAbortCondition () {
            return null;
        }

        public virtual void End () {
            throw new System.NotImplementedException();
        }

        protected virtual TaskStatus OnUpdate () {
            return TaskStatus.Success;
        }

        public virtual void Reset (bool hardReset = false) {
            if (children.Count <= 0) return;
            foreach (var child in children) {
                child.Reset(hardReset);
            }
        }

        public virtual ITaskParent AddChild (ITask child) {
            if (!child.Enabled) {
                return this;
            }
            
            if (children.Count < MaxChildren || MaxChildren < 0) {
                children.Add(child);
            }

            return this;
        }

        public virtual TaskStatus GetAbortStatus () {
            throw new System.NotImplementedException();
        }
    }
}