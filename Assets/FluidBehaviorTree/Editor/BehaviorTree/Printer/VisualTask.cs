using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class VisualTask {
        private float _width = 50;
        private float _height = 50;
        
        private List<VisualTask> _children = new List<VisualTask>();
        private ITreeBox _box;
        private ITask _task;

        public VisualTask (ITask task, ITreeContainer parentContainer) {
            _task = task;
            
            var container = new TreeContainerVertical();
            var childContainer = new TreeContainerHorizontal();

            _box = new TreeBox();
            _box.SetSize(50, 50);
            
            parentContainer.AddBox(container);
            container.AddBox(_box);

            if (task.Children != null) {
                foreach (var child in task.Children) {
                    _children.Add(new VisualTask(child, childContainer));
                }
            }

            container.AddBox(childContainer);

            Debug.Log(task.Name);
            Debug.Log($"Parent Global Position: {parentContainer.GlobalPositionX}, {parentContainer.GlobalPositionY}");
            Debug.Log($"Container Global Position: {container.GlobalPositionX}, {container.GlobalPositionY}");
            Debug.Log($"Box Global Position: {_box.GlobalPositionX}, {_box.GlobalPositionY}");
        }

        public void Print () {
            var rect = new Rect(_box.GlobalPositionX, _box.GlobalPositionY, _box.Width, _box.Height);
            GUI.Box(rect, _task.Name);
            
            foreach (var child in _children) {
                child.Print();
            }
        }
    }
}
