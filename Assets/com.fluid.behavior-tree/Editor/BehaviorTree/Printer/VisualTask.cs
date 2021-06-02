using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class VisualTask {
        private readonly List<VisualTask> _children = new List<VisualTask>();
        private readonly NodePrintController _printer;
        private bool _taskActive;

        public ITask Task { get; }
        public IReadOnlyList<VisualTask> Children => _children;
        
        public float Width { get; } = 70;
        public float Height { get; } = 50;
        
        public IGraphBox Box { get; private set; }
        public IGraphBox Divider { get; private set; }
        public float DividerLeftOffset { get; private set; }

        public VisualTask (ITask task, IGraphContainer parentContainer) {
            Task = task;
            BindTask();
            
            var container = new GraphContainerVertical();

            AddBox(container);

            if (task.Children != null) {
                var childContainer = new GraphContainerHorizontal();
                foreach (var child in task.Children) {
                    _children.Add(new VisualTask(child, childContainer));
                }
                
                AddDivider(container, childContainer);
                container.AddBox(childContainer);
            }

            parentContainer.AddBox(container);
            
            _printer = new NodePrintController(this);
        }

        private void BindTask () {
            Task.EditorUtils.EventActive.AddListener(UpdateTaskActiveStatus);
        }

        public void RecursiveTaskUnbind () {
            Task.EditorUtils.EventActive.RemoveListener(UpdateTaskActiveStatus);
            
            foreach (var child in _children) {
                child.RecursiveTaskUnbind();
            }
        }

        private void UpdateTaskActiveStatus () {
            _taskActive = true;
        }

        private void AddDivider (IGraphContainer parent, IGraphContainer children) {
            Divider = new GraphBox {
                SkipCentering = true,
            };

            DividerLeftOffset = children.ChildContainers[0].Width / 2;
            var dividerRightOffset = children.ChildContainers[children.ChildContainers.Count - 1].Width / 2;
            var width = children.Width - DividerLeftOffset - dividerRightOffset;

            Divider.SetSize(width, 1);

            parent.AddBox(Divider);
        }

        private void AddBox (IGraphContainer parent) {
            Box = new GraphBox();
            Box.SetSize(Width, Height);
            Box.SetPadding(10, 10);
            parent.AddBox(Box);
        }

        public void Print () {
            _printer.Print(_taskActive);
            _taskActive = false;

            foreach (var child in _children) {
                child.Print();
            }
        }
    }
}
