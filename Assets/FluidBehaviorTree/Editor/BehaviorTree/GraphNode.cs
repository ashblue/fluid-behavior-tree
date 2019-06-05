using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class GraphNode {
        public Vector2 Position { get; private set; }
        public List<GraphNode> Children { get; } = new List<GraphNode>();
        public Vector2 Size { get; }
        
        public GraphNode (ITask task, Vector2 size) {
            Size = size;

            if (task.Children == null) return;
            foreach (var child in task.Children) {
                Children.Add(new GraphNode(child, size));
            }
        }

        public void SetPosition (Vector2 position) {
            Position = position;

            for (var i = 0; i < Children.Count; i++) {
                var child = Children[i];
                var childPos = new Vector2(position.x, position.y + Size.y);
                
                if (Children.Count > 1) {
                    var sidePercent = i / (Children.Count - 1f);
                    if (Mathf.Abs(sidePercent - 0.5f) > 0.01f) {
                        var side = sidePercent > 0.5f ? 1 : -1;
                        var shiftAmount = (Children.Count - 1) / 2f;
                        var shift = Size.x * shiftAmount * side;
                        childPos.x += shift;
                    }
                }

                child.SetPosition(childPos);
            }
        }
    }
}
