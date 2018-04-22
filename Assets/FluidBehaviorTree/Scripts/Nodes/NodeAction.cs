using Adnc.FluidBT.Testing;

namespace FluidBehaviorTree.Scripts.Nodes {
    public class NodeAction : INodeAction {
        private NodeStatus _lastStatus;
        public bool Enabled { get; set; } = true;

        public NodeStatus Update () {
            if (_lastStatus != NodeStatus.Continue) {
                Start();
            }

            _lastStatus = OnUpdate();

            return _lastStatus;
        }

        public virtual NodeStatus OnUpdate () {
            return NodeStatus.Failure;
        }

        public void Awake () {
        }

        public void Start () {
            OnStart();
        }

        public virtual void OnStart () {
        }
    }
}