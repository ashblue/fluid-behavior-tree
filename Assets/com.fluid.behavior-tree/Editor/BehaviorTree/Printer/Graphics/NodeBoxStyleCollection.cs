using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class GuiStyleCollection
    {
        public NodeBoxStyle BoxActive { get; } = new(Color.gray, Color.white);

        public NodeBoxStyle BoxInactive { get; } = new(
            new Color32(150, 150, 150, 255),
            new Color32(208, 208, 208, 255));

        public GUIStyle Title { get; } = new(GUI.skin.label)
        {
            fontSize = 9,
            alignment = TextAnchor.LowerCenter,
            wordWrap = true,
            padding = new RectOffset(3, 3, 3, 3)
        };
    }
}
