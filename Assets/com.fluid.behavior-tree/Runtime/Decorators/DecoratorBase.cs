using System;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
#if UNITY_2021_3_OR_NEWER
using UnityEngine;
#endif

namespace CleverCrow.Fluid.BTs.Decorators
{
    public abstract class DecoratorBase : GenericTaskBase, ITaskParent
    {
        public ITask Child => Children.Count > 0 ? Children[0] : null;
        public List<ITask> Children { get; } = new(); // TODO: Consider adding default size

        public string Name { get; set; }

        public bool IsEnabled { get; set; } = true;

#if UNITY_2021_3_OR_NEWER
        public GameObject Owner { get; set; }
#endif

        public IBehaviorTree ParentTree { get; set; }
        public TaskStatus LastStatus { get; private set; }

        public override TaskStatus Update()
        {
            base.Update();

            if (Child == null)
            {
                var warningText = $"Decorator {Name} has no child. Force returning failure. Please fix";

#if UNITY_2021_3_OR_NEWER
                if (Application.isPlaying)
                {
                    Debug.LogWarning(warningText);
                }
#else
                Console.WriteLine($"Warning: {warningText}");
#endif
                return TaskStatus.Failure;
            }

            var status = OnUpdate();
            LastStatus = status;

            return status;
        }

        public void End()
        {
            Child.End();
        }

        public void Reset()
        {
            // TODO: implement
        }

        public ITaskParent AddChild(ITask child)
        {
            if (Child == null)
            {
                Children.Add(child);
            }

            return this;
        }

        protected abstract TaskStatus OnUpdate();
    }
}
