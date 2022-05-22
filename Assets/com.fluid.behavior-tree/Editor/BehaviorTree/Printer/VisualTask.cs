using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees.Utils;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class VisualTask
    {
        private const float DefaultWidth = 70f;
        private const float DefaultHeight = 50f;

        public ITask Task { get; }
        // TODO: Consider fixing. This can be casted into IList or into List and loose its readonly status
        public IReadOnlyList<VisualTask> Children => _children;

        public float Width { get; } = DefaultWidth;
        public float Height { get; } = DefaultHeight;

        public IGraphBox Box { get; private set; }
        public IGraphBox Divider { get; private set; }
        public float DividerLeftOffset { get; private set; }


        private readonly List<VisualTask> _children = new();
        private readonly NodePrintController _printer;
        private bool _taskActive;

        public VisualTask(ITask task, IGraphContainer parentContainer)
        {
            Task = task;
            BindTask();

            var container = new GraphContainerVertical();

            AddBox(container);

            if (task.Children != null)
            {
                var childContainer = new GraphContainerHorizontal();

                foreach (var child in task.Children)
                {
                    _children.Add(new VisualTask(child, childContainer));
                }

                AddDivider(container, childContainer);
                container.AddBox(childContainer);
            }

            parentContainer.AddBox(container);

            _printer = new NodePrintController(this);
        }

        private void BindTask()
        {
            Task.DebugHandler.SubscribeToEventActive(UpdateTaskActiveStatus);
        }

        public void RecursiveTaskUnbind()
        {
            Task.DebugHandler.UnsubscribeFromEventActive(UpdateTaskActiveStatus);

            foreach (var child in _children)
            {
                child.RecursiveTaskUnbind();
            }
        }

        private void UpdateTaskActiveStatus()
        {
            _taskActive = true;
        }

        private void AddDivider(IGraphContainer parent, IGraphBox children)
        {
            Divider = new GraphBox
            {
                SkipCentering = true
            };

            DividerLeftOffset = children.ChildContainers[0].Width.Half();
            var dividerRightOffset = children.ChildContainers[^1].Width.Half();
            var width = children.Width - DividerLeftOffset - dividerRightOffset;

            Divider.SetSize(width, 1);

            parent.AddBox(Divider);
        }

        private void AddBox(IGraphContainer parent)
        {
            Box = new GraphBox();
            Box.SetSize(Width, Height);
            Box.SetPadding(10, 10);
            parent.AddBox(Box);
        }

        public void Print()
        {
            _printer.Print(_taskActive);
            _taskActive = false;

            foreach (var child in _children)
            {
                child.Print();
            }
        }
    }
}
