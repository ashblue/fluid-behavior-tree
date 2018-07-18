using UnityEngine;

namespace Adnc.FluidBT.Tasks {
    public abstract class TaskBase : ITask {
        private bool _init;
        private bool _start;
        private bool _exit;
        
        public bool Enabled { get; set; } = true;
        public GameObject Owner { get; set; }

        public TaskStatus LastStatus { get; private set; }

        public TaskStatus Update () {
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

            // Soft reset since the node has completed
            if (status != TaskStatus.Continue) {
                Exit();
            }

            return status;
        }

        /// <summary>
        /// Reset the node to be re-used
        /// </summary>
        /// <param name="hardReset">Used to wipe the node back to its original state. Meant for pooling.</param>
        public void Reset (bool hardReset = false) {
            _start = false;
            _exit = false;

            if (hardReset) {
                _init = false;
            }
        }

        public void End () {
            Exit();
        }

        protected virtual TaskStatus GetUpdate () {
            return TaskStatus.Failure;
        }

        private void Init () {
            OnInit();
        }

        /// <summary>
        /// Run the first time this node is run or after a hard reset
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

        /// <summary>
        /// Triggered when this node is complete
        /// </summary>
        private void Exit () {
            if (_exit) {
                OnExit();
            }

            _exit = false;
        }

        protected virtual void OnExit () {
        }
    }
}