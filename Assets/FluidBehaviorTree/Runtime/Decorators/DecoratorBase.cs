using System.Collections.Generic;
using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Trees;
using UnityEngine;

namespace Adnc.FluidBT.Decorators {
    public abstract class DecoratorBase : ITaskParent {
        public List<ITask> Children { get; } = new List<ITask>();

        public string Name { get; set; }

        public bool Enabled { get; set; } = true;

        public GameObject Owner { get; set; }
        public BehaviorTree ParentTree { get; set; }
        public TaskStatus LastStatus { get; private set; }

        public ITask Child => Children.Count > 0 ? Children[0] : null;

        public TaskStatus Update () {
            var status = OnUpdate();
            LastStatus = status;

            return status;
        }

        protected abstract TaskStatus OnUpdate ();

        public void End () {
            Child.End();
        }

        public void Reset () {
        }

        public ITaskParent AddChild (ITask child) {
            if (Child == null) {
                Children.Add(child);
            }

            return this;
        }
    }
}