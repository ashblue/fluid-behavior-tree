namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class BehaviorTreePrinter {
        private readonly VisualTask _root;
        
        public BehaviorTreePrinter (IBehaviorTree tree) {
            var container = new GraphContainerVertical();
            // @TODO Temporary start position for debugging, replace with a proper scroll view
            container.SetGlobalPosition(40, 40);
            _root = new VisualTask(tree.Root, container);
            container.CenterAlignChildren();
        }

        public void Print () {
            _root.Print();
        }
    }
}