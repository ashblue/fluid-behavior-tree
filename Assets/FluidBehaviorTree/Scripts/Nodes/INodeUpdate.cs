using Adnc.FluidBT.Testing;

namespace FluidBehaviorTree.Scripts.Nodes {
    public interface INodeUpdate {
        bool Enabled { get; set; }
        NodeStatus Update ();
    }
}