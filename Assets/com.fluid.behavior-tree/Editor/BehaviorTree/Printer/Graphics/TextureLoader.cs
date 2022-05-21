using CleverCrow.Fluid.BTs.Trees.Utils;
using UnityEditor;
using UnityEngine;
using static UnityEditor.AssetDatabase;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class TextureLoader
    {
        public Texture2D Texture { get; }

        public TextureLoader(string spritePath)
        {
            // TODO: Convert replace value into const
            Texture = LoadAssetAtPath<Texture2D>(spritePath.Replace(PathUtils.RootPlaceholder, AssetPath.BasePath));
        }

        public void Paint(Rect rect, Color color)
        {
            var oldColor = GUI.color;
            GUI.color = color;

            if (!Texture)
            {
                return;
            }

            GUI.Label(rect, Texture);

            GUI.color = oldColor;
        }
    }
}
