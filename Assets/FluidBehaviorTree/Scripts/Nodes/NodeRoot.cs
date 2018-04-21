using Adnc.FluidBT.Testing;

namespace FluidBehaviorTree.Scripts.Nodes {
    public class NodeRoot : INodeRoot {
        public INodeUpdate Child { get; set; }

        public bool Enabled { get; set; } = true;

        public NodeStatus Update () {
            if (Child != null && Child.Enabled) {
                return Child.Update();
            }

            return NodeStatus.Failure;
        }
    }
}