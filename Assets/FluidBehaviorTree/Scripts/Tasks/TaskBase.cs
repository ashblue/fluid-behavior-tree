using Adnc.FluidBT.Trees;

namespace Adnc.FluidBT.Tasks {
    public abstract class TaskBase : ITask {
        private bool _init;
        private bool _start;
        private bool _exit;
        
        public bool Enabled { get; set; } = true;

        public TaskStatus LastStatus { get; private set; }

        public BehaviorTree Owner { get; set; }

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

        private void Start () {
            OnStart();
        }

        protected virtual void OnStart () {
        }

        private void Init () {
            OnInit();
        }

        protected virtual void OnInit () {
        }

        /// <summary>
        /// Exit can only be called if the exit bool has been activated (to safeguard against End() usage)
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