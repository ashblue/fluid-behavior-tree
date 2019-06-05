using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class BehaviorTreePrinter {
        private readonly GraphNode _root;
        
        public BehaviorTreePrinter (IBehaviorTree tree) {
            _root = new GraphNode(tree.Root, new GraphNodePrinter(), new GraphNodeOptions {
                Size = new Vector2(50, 50),
                VerticalConnectorTopHeight = 3,
                VerticalConnectorBottomHeight = 7,
                HorizontalConnectorHeight = 1,
            });
            
            // @TODO Temporary start position for debugging, replace with a proper scroll view
            _root.SetPosition(new Vector2(300, 0));
        }

        public void Print () {
            _root.Print();
        }
    }
}