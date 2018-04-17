using FluidBehaviorTree.Scripts.Nodes;

namespace Adnc.FluidBT.Trees {
    public class BehaviorTree {
        private readonly NodeRoot _root = new NodeRoot();
        public NodeRoot root {
            get { return _root; }
        }

        public NodeRoot current { get; private set; }

        public BehaviorTree () {
            current = root;
        }
    }
}