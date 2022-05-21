using System;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.TaskParents
{
    public abstract class TaskParentBase : GenericTaskBase, ITaskParent
    {
        private int _lastTickCount;

        protected virtual int MaxChildren { get; } = -1;

        public IBehaviorTree ParentTree { get; set; }
        public TaskStatus LastStatus { get; private set; }

        public virtual string Name { get; set; }
        public bool Enabled { get; set; } = true;

        public List<ITask> Children { get; } = new();

        public GameObject Owner { get; set; }

        public override TaskStatus Update()
        {
            base.Update();
            UpdateTicks();

            var status = OnUpdate();
            LastStatus = status;
            if (status != TaskStatus.Continue) Reset();

            return status;
        }

        public virtual void End()
        {
            throw new NotImplementedException();
        }

        public virtual void Reset()
        {
        }

        public virtual ITaskParent AddChild(ITask child)
        {
            if (!child.Enabled) return this;

            if (Children.Count < MaxChildren || MaxChildren < 0) Children.Add(child);

            return this;
        }

        private void UpdateTicks()
        {
            if (ParentTree == null) return;

            if (_lastTickCount != ParentTree.TickCount) Reset();

            _lastTickCount = ParentTree.TickCount;
        }

        protected virtual TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
