using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class TaskBase : GenericTaskBase {
        private bool _init;
        private bool _start;
        private bool _exit;
        private int _lastTickCount;
        private bool _active;

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

            var status = GetUpdate();
            LastStatus = status;

            if (status != TaskStatus.Continue) {
                if (_active) ParentTree?.RemoveActiveTask(this);
                Exit();
            } else if (!_active) {
                ParentTree?.AddActiveTask(this);
                _active = true;
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

        /// <summary>
        /// Reset the node to be re-used
        /// </summary>
        public override void Reset () {
            _active = false;
            _start = false;
            _exit = false;
        }

        public override void End () {
            Exit();
        }

        protected virtual TaskStatus GetUpdate () {
            return TaskStatus.Failure;
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
        /// Triggered when this node is complete
        /// </summary>
        protected virtual void OnExit () {
        }
    }
}
