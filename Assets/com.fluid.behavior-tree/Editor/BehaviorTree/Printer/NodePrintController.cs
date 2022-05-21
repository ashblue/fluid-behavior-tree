using System.Linq;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Trees.Utils;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class NodePrintController
    {
        private static GuiStyleCollection Styles => BehaviorTreePrinter.SharedStyles;

        private static readonly Color LastStatusIconColor = new Color(1, 1, 1, 0.7f);

        private readonly VisualTask _node;
        private readonly IGraphBox _box;
        private readonly IGraphBox _divider;
        private readonly NodeFaders _faders = new();

        private readonly TextureLoader _iconMain;

        private Texture2D _dividerGraphic;
        private Texture2D _verticalBottom;
        private Texture2D _verticalTop;

        public NodePrintController(VisualTask node)
        {
            _node = node;
            _box = node.Box;
            _divider = node.Divider;
            _iconMain = new TextureLoader(_node.Task.IconPath);
        }

        public void Print(bool taskIsActive)
        {
            if (_node.Task is not TaskRoot)
            {
                PaintVerticalTop();
            }

            _faders.Update(taskIsActive);

            PaintBody();

            if (_node.Children.Count <= 0)
            {
                return;
            }

            PaintDivider();
            PaintVerticalBottom();
        }

        private void PaintBody()
        {
            var prevBackgroundColor = GUI.backgroundColor;

            var rect = new Rect(
                _box.GlobalPositionX + _box.PaddingX,
                _box.GlobalPositionY + _box.PaddingY,
                _box.Width - _box.PaddingX,
                _box.Height - _box.PaddingY
            );

            if (_node.Task.HasBeenActive)
            {
                GUI.backgroundColor = _faders.BackgroundFader.CurrentColor;
                GUI.Box(rect, GUIContent.none, Styles.BoxActive.Style);
                GUI.backgroundColor = prevBackgroundColor;

                PrintLastStatus(rect);
            }
            else
            {
                GUI.Box(rect, GUIContent.none, Styles.BoxInactive.Style);
            }

            PrintIcon();

            Styles.Title.normal.textColor = _faders.TextFader.CurrentColor;
            GUI.Label(rect, _node.Task.Name, Styles.Title);
        }


        private void PrintLastStatus(Rect rect)
        {
            const float sidePadding = 1.5f;

            var icon = BehaviorTreePrinter.StatusIcons.GetIcon(_node.Task.LastStatus);

            var iconRect = new Rect(
                rect.x + rect.size.x - icon.Texture.width - sidePadding,
                rect.y + rect.size.y - icon.Texture.height - sidePadding,
                icon.Texture.width,
                icon.Texture.height
            );

            icon.Paint(iconRect, LastStatusIconColor);
        }

        private void PrintIcon()
        {
            const float iconWidth = 35f;
            const float iconHeight = 35f;
            const float paddingY = 3f;

            var x = _box.GlobalPositionX + _box.PaddingX.Half() + _box.Width.Half() - iconWidth.Half() +
                    _node.Task.IconPadding.Half();
            var y = _box.GlobalPositionY + _box.PaddingX.Half() + paddingY + _node.Task.IconPadding.Half();
            var width = iconWidth - _node.Task.IconPadding;
            var height = iconHeight - _node.Task.IconPadding;

            _iconMain.Paint(
                new Rect(x, y, width, height),
                _faders.MainIconFader.CurrentColor
            );
        }

        private void PaintDivider()
        {
            const int graphicSizeIncrease = 5;
            const float dividerOffsetX = 2f;
            const float height = 10f;

            if (!_dividerGraphic)
            {
                _dividerGraphic = CreateTexture(
                    (int)_divider.Width + graphicSizeIncrease,
                    1,
                    Color.black
                );
            }

            var position = new Rect(
                _divider.GlobalPositionX + _box.PaddingY.Half() + _node.DividerLeftOffset - dividerOffsetX,
                // @TODO Should not need to offset this
                _divider.GlobalPositionY + _box.PaddingY.Half(),
                _divider.Width + graphicSizeIncrease,
                height
            );

            GUI.Label(position, _dividerGraphic);
        }

        private void PaintVerticalBottom()
        {
            const int defaultWidth = 1;

            if (!_verticalBottom)
            {
                _verticalBottom = CreateTexture(defaultWidth, (int)_box.PaddingY, Color.black);
            }

            const float width = 100f;
            const float paddingX = 2f;
            const float paddingY = 1f;

            var position = new Rect(
                _box.GlobalPositionX + _node.Width.Half() + _box.PaddingX - paddingX,
                _box.GlobalPositionY + _node.Height + _box.PaddingY - paddingY,
                width,
                _box.PaddingY - paddingY
            );

            GUI.Label(position, _verticalBottom);
        }

        private void PaintVerticalTop()
        {
            const int defaultWidth = 1;

            if (!_verticalTop)
            {
                _verticalTop = CreateTexture(defaultWidth, Mathf.RoundToInt(_box.PaddingY.Half()), Color.black);
            }

            const float paddingX = 2f;
            const float width = 100f;
            const float height = 10f;

            var position = new Rect(
                _box.GlobalPositionX + _node.Width.Half() + _box.PaddingX - paddingX,
                _box.GlobalPositionY + _box.PaddingY.Half(),
                width,
                height
            );

            GUI.Label(position, _verticalTop);
        }

        private static Texture2D CreateTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.SetPixels(Enumerable.Repeat(color, width * height).ToArray());
            texture.Apply();

            return texture;
        }
    }
}
