using System.Collections.Generic;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Trees;
using UnityEngine;

namespace Adnc.FluidBT.TaskParents {
    public abstract class TaskParentBase : ITaskParent {
        public TaskStatus LastStatus { get; private set; }

        public bool Enabled { get; set; } = true;

        public List<ITask> Children { get; } = new List<ITask>();

        protected virtual int MaxChildren { get; } = -1;

        public GameObject Owner { get; set; }

        public TaskStatus Update () {
            var status = OnUpdate();
            LastStatus = status;

            return status;
        }

        public virtual void End () {
            throw new System.NotImplementedException();
        }

        protected virtual TaskStatus OnUpdate () {
            return TaskStatus.Success;
        }

        public virtual void Reset (bool hardReset = false) {
            if (Children.Count <= 0) return;
            foreach (var child in Children) {
                child.Reset(hardReset);
            }
        }

        public virtual ITaskParent AddChild (ITask child) {
            if (!child.Enabled) {
                return this;
            }
            
            if (Children.Count < MaxChildren || MaxChildren < 0) {
                Children.Add(child);
            }

            return this;
        }
    }
}