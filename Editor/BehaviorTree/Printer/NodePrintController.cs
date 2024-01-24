using System.Linq;
using CleverCrow.Fluid.BTs.TaskParents;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class NodePrintController {
        private readonly VisualTask _node;
        private readonly IGraphBox _box;
        private readonly IGraphBox _divider;
        private readonly NodeFaders _faders = new NodeFaders();

        private readonly TextureLoader _iconMain;

        private Texture2D _dividerGraphic;
        private Texture2D _verticalBottom;
        private Texture2D _verticalTop;

        private static GuiStyleCollection Styles => BehaviorTreePrinter.SharedStyles;

        public NodePrintController (VisualTask node) {
            _node = node;
            _box = node.Box;
            _divider = node.Divider;
            _iconMain = new TextureLoader(_node.Task.IconPath);
        }
        
        public void Print (bool taskIsActive) {
            if (!(_node.Task is TaskRoot)) PaintVerticalTop();
            _faders.Update(taskIsActive);
            
            PaintBody();
            
            if (_node.Children.Count > 0) {
                PaintDivider();
                PaintVerticalBottom();
            }
        }

        private void PaintBody () {
            var prevBackgroundColor = GUI.backgroundColor;
            
            var rect = new Rect(
                _box.GlobalPositionX + _box.PaddingX, 
                _box.GlobalPositionY + _box.PaddingY,
                _box.Width - _box.PaddingX, 
                _box.Height - _box.PaddingY);

            if (_node.Task.HasBeenActive) {
                GUI.backgroundColor = _faders.BackgroundFader.CurrentColor;
                GUI.Box(rect, GUIContent.none, Styles.BoxActive.Style);
                GUI.backgroundColor = prevBackgroundColor;
                
                PrintLastStatus(rect);
            } else {
                GUI.Box(rect, GUIContent.none, Styles.BoxInactive.Style);
            }
            
            PrintIcon();

            Styles.Title.normal.textColor = _faders.TextFader.CurrentColor;
            GUI.Label(rect, _node.Task.Name, Styles.Title);
        }

        private void PrintLastStatus (Rect rect) {
            const float sidePadding = 1.5f;

            var icon = BehaviorTreePrinter.StatusIcons.GetIcon(_node.Task.LastStatus);
            icon.Paint(
                new Rect(
                    rect.x + rect.size.x - icon.Texture.width - sidePadding,
                    rect.y + rect.size.y - icon.Texture.height - sidePadding,
                    icon.Texture.width, icon.Texture.height),
                new Color(1, 1, 1, 0.7f));
        }

        private void PrintIcon () {
            const float iconWidth = 35;
            const float iconHeight = 35;
            _iconMain.Paint(
                new Rect(
                    _box.GlobalPositionX + _box.PaddingX / 2 + _box.Width / 2 - iconWidth / 2 + _node.Task.IconPadding / 2,
                    _box.GlobalPositionY + _box.PaddingX / 2 + 3 + _node.Task.IconPadding / 2,
                    iconWidth - _node.Task.IconPadding,
                    iconHeight - _node.Task.IconPadding),
                _faders.MainIconFader.CurrentColor);
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
