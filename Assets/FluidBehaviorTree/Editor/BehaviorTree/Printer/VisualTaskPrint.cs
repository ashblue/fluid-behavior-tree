using System.Linq;
using CleverCrow.Fluid.BTs.TaskParents;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class VisualTaskPrint {
        private readonly VisualTask _node;
        private ColorFader _colorFader = new ColorFader();
        
        private IGraphBox _box;
        private IGraphBox _divider;

        private GUIStyle _boxStyle;
        private Texture2D _dividerGraphic;
        private Texture2D _verticalBottom;
        private Texture2D _verticalTop;
        private Texture2D _boxBorder;

        public VisualTaskPrint (VisualTask node) {
            _node = node;
            _box = node.Box;
            _divider = node.Divider;
            _colorFader = new ColorFader();
            
            CreateBoxStyle();
        }
        
        public void Print (bool taskIsActive) {
            if (!(_node.Task is TaskRoot)) PaintVerticalTop();
            
            _colorFader.Update(taskIsActive);
            PaintBody();
            
            if (_node.Children.Count > 0) {
                PaintDivider();
                PaintVerticalBottom();
            }
        }

        private void CreateBoxStyle () {
            _boxBorder = CreateTexture(19, 19, Color.gray);
            _boxBorder.SetPixels(1, 1, 17, 17, Enumerable.Repeat(Color.white, 17 * 17).ToArray());
            _boxBorder.Apply();
            _boxStyle = new GUIStyle(GUI.skin.box) {
                border = new RectOffset(1, 1, 1, 1),
                normal = {
                    background = _boxBorder
                }
            };
        }

        private void PaintBody () {
            var rect = new Rect(
                _box.GlobalPositionX + _box.PaddingX, 
                _box.GlobalPositionY + _box.PaddingY,
                _box.Width - _box.PaddingX, 
                _box.Height - _box.PaddingY);

            var prevBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = _colorFader.CurrentColor;
            GUI.Box(rect, _node.Task.Name, _boxStyle);
            GUI.backgroundColor = prevBackgroundColor;
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
                _divider.GlobalPositionX + _box.PaddingY / 2 + _node.DividerLeftOffset - 2, 
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
                _box.GlobalPositionX + _node.Width / 2 + _box.PaddingX - 2, 
                _box.GlobalPositionY + _node.Height + _box.PaddingY - 1,
                100, 
                _box.PaddingY - 1);
            
            GUI.Label(position, _verticalBottom);
        }
        
        private void PaintVerticalTop () {
            if (_verticalTop == null) {
                _verticalTop = CreateTexture(1, Mathf.RoundToInt(_box.PaddingY / 2), Color.black);
            }

            var position = new Rect(
                _box.GlobalPositionX + _node.Width / 2 + _box.PaddingX - 2, 
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
