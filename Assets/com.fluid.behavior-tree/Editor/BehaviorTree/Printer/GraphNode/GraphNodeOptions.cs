using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class GraphNodeOptions
    {
        private const float DefaultX = 50f;
        private const float DefaultY = 100f;

        public int VerticalConnectorBottomHeight { get; set; }
        public int HorizontalConnectorHeight { get; set; }
        public int VerticalConnectorTopHeight { get; set; }
        public Vector2 Size { get; set; } = new(DefaultX, DefaultY);
    }
}
