using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class VisualTask {
        private Texture2D _verticalBottom;
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
            _box.SetPadding(10, 10);
            
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
            if (!(_task is TaskRoot)) PaintVerticalTop();
            PaintBody();
            if (_children.Count > 0) PaintVerticalBottom();

            foreach (var child in _children) {
                child.Print();
            }
        }

        private void PaintBody () {
            var rect = new Rect(
                _box.GlobalPositionX + _box.PaddingX, 
                _box.GlobalPositionY + _box.PaddingY,
                _box.Width - _box.PaddingX, 
                _box.Height - _box.PaddingY);
            
            GUI.Box(rect, _task.Name);
        }

        private void PaintVerticalBottom () {
            if (_verticalBottom == null) {
                _verticalBottom = CreateTexture(1, Mathf.RoundToInt(_box.PaddingY / 2), Color.black);
            }

            var position = new Rect(
                _box.GlobalPositionX + _width / 2 + _box.PaddingX - 2, 
                _box.GlobalPositionY + _height + _box.PaddingY - 1,
                100, 
                100);
            
            GUI.Label(position, _verticalBottom);
        }
        
        private void PaintVerticalTop () {
            if (_verticalBottom == null) {
                _verticalBottom = CreateTexture(1, Mathf.RoundToInt(_box.PaddingY / 2), Color.black);
            }

            var position = new Rect(
                _box.GlobalPositionX + _width / 2 + _box.PaddingX - 2, 
                _box.GlobalPositionY + _box.PaddingY / 2,
                100, 
                100);
            
            GUI.Label(position, _verticalBottom);
        }
        
        private static Texture2D CreateTexture (int width, int height, Color color) {
            var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.SetPixels(Enumerable.Repeat(color, width * height).ToArray());
            texture.Apply();
            
            return texture;
        }
    }
}
