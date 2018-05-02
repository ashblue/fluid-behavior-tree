namespace Adnc.FluidBT.Tasks {
    public abstract class TaskBase : ITask {
        private bool _init;
        private bool _start;
        
        public bool Enabled { get; set; } = true;

        public bool IsLowerPriority { get; } = false;

        public TaskStatus Update () {
            if (!_init) {
                Init();
                _init = true;
            }
            
            if (!_start) {
                Start();
                _start = true;
            }

            var status = GetUpdate();

            // Soft reset since the node has completed
            if (status != TaskStatus.Continue) {
                Exit();
            }

            return status;
        }

        public virtual ITask GetAbortCondition () {
            return null;
        }

        /// <summary>
        /// Reset the node to be re-used
        /// </summary>
        /// <param name="hardReset">Used to wipe the node back to its original state. Meant for pooling.</param>
        public void Reset (bool hardReset = false) {
            _start = false;

            if (hardReset) {
                _init = false;
            }
        }

        public TaskStatus GetAbortStatus () {
            return Update();
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

        private void Exit () {
            OnExit();
        }

        protected virtual void OnExit () {
        }
    }
}