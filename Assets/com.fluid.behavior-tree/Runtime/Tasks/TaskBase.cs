using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Tasks
{
    public abstract class TaskBase : GenericTaskBase, ITask
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; } = true;
        public GameObject Owner { get; set; }
        public IBehaviorTree ParentTree { get; set; }

        public List<ITask> Children => null;
        public TaskStatus LastStatus { get; private set; }

        private bool _isActive;
        private bool _isExit;
        private bool _isInitiated;
        private int _lastTickCount;
        private bool _isStart;

        public override TaskStatus Update()
        {
            base.Update();
            UpdateTicks();

            if (!_isInitiated)
            {
                Init();
                _isInitiated = true;
            }

            if (!_isStart)
            {
                Start();
                _isStart = true;
                _isExit = true;
            }

            var status = GetUpdate();
            LastStatus = status;

            if (status != TaskStatus.Continue)
            {
                if (_isActive)
                {
                    ParentTree?.RemoveActiveTask(this);
                }

                Exit();
            }
            else if (!_isActive)
            {
                ParentTree?.AddActiveTask(this);
                _isActive = true;
            }

            return status;
        }

        /// <summary>
        ///     Reset the node to be re-used
        /// </summary>
        public void Reset()
        {
            _isActive = false;
            _isStart = false;
            _isExit = false;
        }

        public void End()
        {
            Exit();
        }

        private void UpdateTicks()
        {
            if (ParentTree == null)
            {
                return;
            }

            if (_lastTickCount != ParentTree.TickCount)
            {
                Reset();
            }

            _lastTickCount = ParentTree.TickCount;
        }

        protected virtual TaskStatus GetUpdate() => TaskStatus.Failure;

        private void Init() => OnInit();

        /// <summary>
        /// Triggers the first time this node is run or after a hard reset
        /// </summary>
        protected virtual void OnInit()
        {
        }

        private void Start() => OnStart();

        /// <summary>
        /// Run every time this node begins
        /// </summary>
        protected virtual void OnStart()
        {
        }

        private void Exit()
        {
            if (_isExit)
            {
                OnExit();
            }

            Reset();
        }

        /// <summary>
        /// Triggered when this node is complete
        /// </summary>
        protected virtual void OnExit()
        {
        }
    }
}
