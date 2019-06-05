using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class GraphNode {
        private IGraphNodePrinter _printer;
        
        public Vector2 Position { get; private set; }
        public List<GraphNode> Children { get; } = new List<GraphNode>();
        public Vector2 Size { get; }
        public ITask Task { get; }

        public GraphNode (ITask task, Vector2 size, IGraphNodePrinter printer) {
            _printer = printer;
            
            Size = size;
            Task = task;

            if (task.Children == null) return;
            foreach (var child in task.Children) {
                Children.Add(new GraphNode(child, size, printer));
            }
        }

        public void SetPosition (Vector2 position) {
            Position = position;

            for (var i = 0; i < Children.Count; i++) {
                var child = Children[i];
                var childPos = new Vector2(position.x, position.y + Size.y);
                
                // Center the child, then align it to the expected position
                childPos.x += Size.x / 2 + Size.x * i;
                
                // Shift the child as if it were in a container so it lines up properly
                childPos.x -= Size.x * (Children.Count / 2f);

                child.SetPosition(childPos);
            }
        }

        public void Print () {
            _printer.Print(this);

            foreach (var child in Children) {
                child.Print();
            }
        }
    }
}
