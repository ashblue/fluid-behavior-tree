using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.TaskParents {
    public abstract class TaskParentBase : GenericTaskBase, ITaskParent {
        private bool _init;
        private bool _start;
        private bool _exit;

        private int _lastTickCount;

        public IBehaviorTree ParentTree { get; set; }
        public TaskStatus LastStatus { get; private set; }

        public virtual string Name { get; set; }
        public bool Enabled { get; set; } = true;

        public List<ITask> Children { get; } = new List<ITask>();

        protected virtual int MaxChildren { get; } = -1;

        public GameObject Owner { get; set; }

        public override TaskStatus Update () {
            base.Update();
            UpdateTicks();

            if (!_init) {
                Init();
                _init = true;
            }

            if (!_start) {
                Start();
                _start = true;
                _exit = true;
            }

            var status = OnUpdate();
            LastStatus = status;
            if (status != TaskStatus.Continue) {
                Exit();
            }

            return status;
        }

        private void UpdateTicks () {
            if (ParentTree == null) {
                return;
            }

            if (_lastTickCount != ParentTree.TickCount) {
                Reset();
            }

            _lastTickCount = ParentTree.TickCount;
        }

        private void Init () {
            OnInit();
        }

        /// <summary>
        /// Triggers the first time this node is run or after a hard reset
        /// </summary>
        protected virtual void OnInit () {
        }

        private void Start () {
            OnStart();
        }

        /// <summary>
        /// Run every time this node begins
        /// </summary>
        protected virtual void OnStart () {
        }

        private void Exit () {
            if (_exit) {
                OnExit();
            }
            Reset();
        }

        /// <summary>
        /// Triggered when this node is complete.
        /// </summary>
        protected virtual void OnExit () {
        }

        public virtual void End () {
            throw new System.NotImplementedException();
        }

        protected virtual TaskStatus OnUpdate () {
            return TaskStatus.Success;
        }

        public virtual void Reset () {
            _start = false;
            _exit = false;
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
