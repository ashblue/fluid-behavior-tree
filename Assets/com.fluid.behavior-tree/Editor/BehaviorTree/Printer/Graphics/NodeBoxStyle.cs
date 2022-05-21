using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class NodeBoxStyle
    {
        private const int NodeSize = 19;
        private const int InternalNodeSize = 17;

        public GUIStyle Style { get; }

        public NodeBoxStyle(Color32 border, Color background)
        {
            var texture = CreateTexture(NodeSize, NodeSize, border);
            var colors = Enumerable.Repeat(background, InternalNodeSize * InternalNodeSize).ToArray();

            texture.SetPixels(1, 1, InternalNodeSize, InternalNodeSize, colors);
            texture.Apply();

            const int offsetValue = 1;

            Style = new GUIStyle(GUI.skin.box)
            {
                border = new RectOffset(offsetValue, offsetValue, offsetValue, offsetValue),
                normal =
                {
                    background = texture
                }
            };
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
