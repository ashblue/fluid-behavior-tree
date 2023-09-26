using System.Collections.Generic;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Decorators {
    public abstract class DecoratorBase : ITaskParent {
        public override bool Enabled { get; set; } = true;
        public override List<ITask> Children { get; set; } = new List<ITask>();

        public ITask Child => Children.Count > 0 ? Children[0] : null;

        public override TaskStatus Update () {
            base.Update();

            if (Child == null) {
                if (Application.isPlaying) Debug.LogWarning(
                    $"Decorator {Name} has no child. Force returning failure. Please fix");
                return TaskStatus.Failure;
            }
            
            var status = OnUpdate();
            LastStatus = status;

            return status;
        }

        protected abstract TaskStatus OnUpdate ();

        public override void End () {
            Child.End();
        }

        public override void Reset () {
        }

        public override ITaskParent AddChild (ITask child) {
            if (Child == null) {
                Children.Add(child);
            }

            return this;
        }
    }
}
