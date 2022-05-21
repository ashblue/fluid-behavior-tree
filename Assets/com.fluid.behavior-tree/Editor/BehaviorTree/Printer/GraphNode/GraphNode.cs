using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees.Utils;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class GraphNode
    {
        public float ContainerHeight => GetContainerHeight();
        public Vector2 Position { get; private set; }
        public List<GraphNode> Children { get; } = new();
        public Vector2 Size { get; }
        public ITask Task { get; }
        public int VerticalConnectorBottomHeight { get; }
        public int VerticalConnectorTopHeight { get; }
        public int HorizontalConnectorHeight { get; }

        private readonly IGraphNodePrinter _printer;

        public GraphNode(ITask task, IGraphNodePrinter printer, GraphNodeOptions options)
        {
            _printer = printer;
            Task = task;

            Size = options.Size;
            VerticalConnectorBottomHeight = options.VerticalConnectorBottomHeight;
            VerticalConnectorTopHeight = options.VerticalConnectorTopHeight;
            HorizontalConnectorHeight = options.HorizontalConnectorHeight;

            if (task.Children == null)
            {
                return;
            }

            foreach (var child in task.Children)
            {
                Children.Add(new GraphNode(child, printer, options));
            }
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;

            for (var i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                var childPos = new Vector2(position.x, position.y + ContainerHeight);

                // Center the child, then align it to the expected position
                childPos.x += Size.x.Half() + Size.x * i;

                // Shift the child as if it were in a container so it lines up properly
                childPos.x -= Size.x * (Children.Count.Half());

                child.SetPosition(childPos);
            }
        }

        public void Print()
        {
            _printer.Print(this);

            foreach (var child in Children)
            {
                child.Print();
            }
        }

        private float GetContainerHeight()
        {
            return Size.y + VerticalConnectorBottomHeight + HorizontalConnectorHeight + VerticalConnectorTopHeight;
        }
    }
}
