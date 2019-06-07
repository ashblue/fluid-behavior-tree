using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class VisualTask {
        private Texture2D _dividerGraphic;
        private Texture2D _verticalBottom;
        private Texture2D _verticalTop;

        private float _width = 50;
        private float _height = 50;
        
        private List<VisualTask> _children = new List<VisualTask>();
        private IGraphBox _box;
        private ITask _task;

        private IGraphBox _divider;
        private float _dividerLeftOffset;
        private float _dividerRightOffset;
        
        public VisualTask (ITask task, IGraphContainer parentContainer) {
            _task = task;
            
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
        }

        private void AddDivider (IGraphContainer parent, IGraphContainer children) {
            _divider = new GraphBox {
                SkipCentering = true,
            };

            _dividerLeftOffset = children.ChildContainers[0].Width / 2;
            _dividerRightOffset = children.ChildContainers[children.ChildContainers.Count - 1].Width / 2;
            var width = children.Width - _dividerLeftOffset - _dividerRightOffset;

            _divider.SetSize(width, 1);

            parent.AddBox(_divider);
        }

        private void AddBox (IGraphContainer parent) {
            _box = new GraphBox();
            _box.SetSize(50, 50);
            _box.SetPadding(10, 10);
            parent.AddBox(_box);
        }

        public void Print () {
            if (!(_task is TaskRoot)) PaintVerticalTop();
            PaintBody();
            if (_children.Count > 0) {
                PaintDivider();
                PaintVerticalBottom();
            }

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

        private void PaintDivider () {
            const int graphicSizeIncrease = 5;
            
            if (_dividerGraphic == null) {
                _dividerGraphic = CreateTexture(
                    (int)_divider.Width + graphicSizeIncrease, 
                    1, 
                    Color.black);
            }

            var position = new Rect(
                _divider.GlobalPositionX + _box.PaddingY / 2 + _dividerLeftOffset - 2, 
                // @TODO Should not need to offset this
                _divider.GlobalPositionY + _box.PaddingY / 2,
                _divider.Width + graphicSizeIncrease, 
                10);
            
            GUI.Label(position, _dividerGraphic);
        }

        private void PaintVerticalBottom () {
            if (_verticalBottom == null) {
                _verticalBottom = CreateTexture(1, (int)_box.PaddingY, Color.black);
            }

            var position = new Rect(
                _box.GlobalPositionX + _width / 2 + _box.PaddingX - 2, 
                _box.GlobalPositionY + _height + _box.PaddingY - 1,
                100, 
                _box.PaddingY - 1);
            
            GUI.Label(position, _verticalBottom);
        }
        
        private void PaintVerticalTop () {
            if (_verticalTop == null) {
                _verticalTop = CreateTexture(1, Mathf.RoundToInt(_box.PaddingY / 2), Color.black);
            }

            var position = new Rect(
                _box.GlobalPositionX + _width / 2 + _box.PaddingX - 2, 
                _box.GlobalPositionY + _box.PaddingY / 2,
                100, 
                10);
            
            GUI.Label(position, _verticalTop);
        }
        
        private static Texture2D CreateTexture (int width, int height, Color color) {
            var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.SetPixels(Enumerable.Repeat(color, width * height).ToArray());
            texture.Apply();
            
            return texture;
        }
    }
}
