using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class VisualTask {
        private float _width = 50;
        private float _height = 50;
        
        private List<VisualTask> _children = new List<VisualTask>();
        private IGraphBox _box;
        private ITask _task;

        public VisualTask (ITask task, IGraphContainer parentContainer) {
            _task = task;
            
            var container = new GraphContainerVertical();

            _box = new GraphBox();
            _box.SetSize(50, 50);
            _box.SetPadding(10, 0);
            
            container.AddBox(_box);

            if (task.Children != null) {
                var childContainer = new GraphContainerHorizontal();
                foreach (var child in task.Children) {
                    _children.Add(new VisualTask(child, childContainer));
                }
                container.AddBox(childContainer);
            }

            parentContainer.AddBox(container);
        }

        public void Print () {
            var rect = new Rect(_box.GlobalPositionX + _box.PaddingX, _box.GlobalPositionY + _box.PaddingY, _box.Width - _box.PaddingX, _box.Height - _box.PaddingY);
            GUI.Box(rect, _task.Name);
            
            foreach (var child in _children) {
                child.Print();
            }
        }
    }
}
