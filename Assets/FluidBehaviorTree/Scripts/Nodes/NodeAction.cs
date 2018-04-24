using Adnc.FluidBT.Testing;

namespace FluidBehaviorTree.Scripts.Nodes {
    public class NodeAction : INodeAction {
        private bool _init;
        private bool _start;
        
        public bool Enabled { get; set; } = true;

        public NodeStatus Update () {
            if (!_init) {
                Init();
                _init = true;
            }
            
            if (!_start) {
                Start();
                _start = true;
            }

            var status = OnUpdate();

            // Soft reset since the node has completed
            if (status != NodeStatus.Continue) {
                Exit();

                // @TODO Should this be triggered at the parent level instead?
                Reset();
            }
            
            return status;
        }

        protected virtual NodeStatus OnUpdate () {
            return NodeStatus.Failure;
        }

        public void Awake () {
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
    }
}