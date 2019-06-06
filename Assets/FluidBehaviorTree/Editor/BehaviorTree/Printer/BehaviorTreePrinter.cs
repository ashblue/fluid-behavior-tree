namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class BehaviorTreePrinter {
        private readonly VisualTask _root;
        
        public BehaviorTreePrinter (IBehaviorTree tree) {
            var container = new TreeContainerVertical();
            // @TODO Temporary start position for debugging, replace with a proper scroll view
            container.SetGlobalPosition(300, 0);
            _root = new VisualTask(tree.Root, container);
        }

        public void Print () {
            _root.Print();
        }
    }
}