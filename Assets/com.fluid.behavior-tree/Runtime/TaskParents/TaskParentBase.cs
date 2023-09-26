using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.TaskParents {
    public abstract class TaskParentBase : ITaskParent {
        private int _lastTickCount;

        public override bool Enabled { get; set; } = true;
        public override List<ITask> Children { set; get; } = new List<ITask>();

        protected virtual int MaxChildren { get; } = -1;

        public override TaskStatus Update () {
            base.Update();
            UpdateTicks();

            var status = OnUpdate();
            LastStatus = status;
            if (status != TaskStatus.Continue) {
                ResetTask();
            }

            return status;
        }

        private void UpdateTicks () {
            if (ParentTree == null) {
                return;
            }

            if (_lastTickCount != ParentTree.TickCount) {
                ResetTask();
            }

            _lastTickCount = ParentTree.TickCount;
        }

        public override void End () {
            throw new System.NotImplementedException();
        }

        protected virtual TaskStatus OnUpdate () {
            return TaskStatus.Success;
        }

        public override void ResetTask () {
        }

        public override ITaskParent AddChild (ITask child) {
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
