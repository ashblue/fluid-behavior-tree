using Adnc.FluidBT.Testing;

namespace FluidBehaviorTree.Scripts.Nodes {
    public class NodeAction : INodeUpdate {
        public bool Enabled { get; set; } = true;

        public NodeStatus Update () {
            return NodeStatus.Failure;
        }
    }
}