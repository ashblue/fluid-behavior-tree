using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class ImageHandler {
        public Texture2D Texture { get; }

        public ImageHandler (string spritePath) {
            Texture = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath)?.texture;
        }

        public void Paint (Rect rect) {
            if (Texture == null) return;
            GUI.Label(rect, Texture);
        }
    }
}
