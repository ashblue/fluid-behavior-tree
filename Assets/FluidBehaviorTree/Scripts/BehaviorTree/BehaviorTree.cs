using FluidBehaviorTree.Scripts.Nodes;

namespace Adnc.FluidBT.Trees {
    public class BehaviorTree {
        public INodeRoot Root { get; set; } = new NodeRoot();

        public INodeUpdate Current { get; set; }

        public BehaviorTree () {
            Current = Root;
        }

        public void Tick () {
            // Update must return the currently active node that should be reticked
            // Two different Update methods
                // Tick: Returns the next node to be ticked
                // Update: A status returned by a node (used by nodes that detect ticking)
            Current.Update();
        }
    }
}