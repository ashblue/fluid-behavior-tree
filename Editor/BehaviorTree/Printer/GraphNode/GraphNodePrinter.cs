using System.Linq;
using CleverCrow.Fluid.BTs.TaskParents;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public interface IGraphNodePrinter {
        void Print (GraphNode node);
    }

    public class GraphNodePrinter : IGraphNodePrinter {
        private Texture2D _verticalBottom;
        private Texture2D _verticalTop;

        private static Color LineColor => EditorGUIUtility.isProSkin ? Color.white : Color.black;

        public void Print (GraphNode node) {
            var rect = new Rect(node.Position, node.Size);
            GUI.Box(rect, node.Task.Name);

            PaintVerticalBottom(node, rect);

            if (!(node.Task is TaskRoot)) {
                PaintVerticalTop(node, rect);
            }
        }

        private void PaintVerticalBottom (GraphNode node, Rect nodeRect) {
            if (_verticalBottom == null) _verticalBottom = CreateTexture(1, node.VerticalConnectorBottomHeight, LineColor);
            var verticalBottomRect = new Rect(nodeRect);
            verticalBottomRect.x += node.Size.x / 2 - 0.5f;
            verticalBottomRect.y += node.Size.y;
            GUI.Label(verticalBottomRect, _verticalBottom);
        }

        private void PaintVerticalTop (GraphNode node, Rect nodeRect) {
            if (_verticalTop == null) _verticalTop = CreateTexture(1, node.VerticalConnectorTopHeight, LineColor);
            var verticalTopRect = new Rect(nodeRect);
            verticalTopRect.x += node.Size.x / 2 - 0.5f;
            verticalTopRect.y -= node.VerticalConnectorTopHeight;
            GUI.Label(verticalTopRect, _verticalTop);
        }

        private Texture2D CreateTexture (int width, int height, Color color) {
            var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.SetPixels(Enumerable.Repeat(color, width * height).ToArray());
            texture.Apply();

            return texture;
        }
    }
}
