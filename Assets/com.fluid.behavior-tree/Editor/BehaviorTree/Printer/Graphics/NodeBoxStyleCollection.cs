using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class GuiStyleCollection {
        public NodeBoxStyle BoxActive { get; } = BehaviorTreeDisplayTheme.GetBoxActive();

        public NodeBoxStyle BoxInactive { get; } = BehaviorTreeDisplayTheme.GetBoxInactive();

        public GUIStyle Title { get; } = new(GUI.skin.label) {
            fontSize = 9,
            alignment = TextAnchor.LowerCenter,
            wordWrap = true,
            padding = new RectOffset(3, 3, 3, 3),
        };
    }
}
