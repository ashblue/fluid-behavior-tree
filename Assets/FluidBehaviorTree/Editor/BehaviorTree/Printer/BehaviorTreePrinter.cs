using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class BehaviorTreePrinter {
        private readonly GraphNode _root;
        
        public BehaviorTreePrinter (IBehaviorTree tree) {
            _root = new GraphNode(tree.Root, new Vector2(50, 50), new GraphNodePrinter());
            
            // @TODO Temporary start position for debugging, replace with a proper scroll view
            _root.SetPosition(new Vector2(300, 0));
        }

        public void Print () {
            _root.Print();
        }
    }
}