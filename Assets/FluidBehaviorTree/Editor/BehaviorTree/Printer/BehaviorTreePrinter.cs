namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class BehaviorTreePrinter {
        private readonly PrintNode _root;
        
        public BehaviorTreePrinter (IBehaviorTree tree) {
            _root = new PrintNode(tree.Root);
        }

        public void Print () {
            _root.Print();
        }
    }
}