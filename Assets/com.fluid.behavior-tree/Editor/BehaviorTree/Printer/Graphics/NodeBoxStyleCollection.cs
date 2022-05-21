using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class GuiStyleCollection
    {
        private const int OffsetSize = 3;
        private static readonly Color32 BorderColor = new(150, 150, 150, 255);
        private static readonly Color32 BackgroundColor = new(208, 208, 208, 255);

        public NodeBoxStyle BoxActive { get; } = new(Color.gray, Color.white);

        public NodeBoxStyle BoxInactive { get; } = new(BorderColor, BackgroundColor);

        public GUIStyle Title { get; } = new(GUI.skin.label)
        {
            fontSize = 9,
            alignment = TextAnchor.LowerCenter,
            wordWrap = true,
            padding = new RectOffset(OffsetSize, OffsetSize, OffsetSize, OffsetSize)
        };
    }
}
