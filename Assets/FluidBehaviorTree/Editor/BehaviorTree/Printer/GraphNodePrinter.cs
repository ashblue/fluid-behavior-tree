using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public interface IGraphNodePrinter {
        void Print (GraphNode node);
    }
    
    public class GraphNodePrinter : IGraphNodePrinter {
        public void Print (GraphNode node) {
            GUI.Box(new Rect(node.Position, node.Size), node.Task.Name);
        }
    }
}
