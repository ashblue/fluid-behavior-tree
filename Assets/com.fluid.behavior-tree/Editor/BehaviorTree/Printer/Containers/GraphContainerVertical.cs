using System.Collections.Generic;
using static CleverCrow.Fluid.BTs.Trees.Utils.MathFExtensions;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class GraphContainerVertical : GraphContainerHorizontal
    {
        public override void AddBox(IGraphBox child)
        {
            CalculateChild(child);
            _childContainers.Add(child);
        }

        private void CalculateChild(IGraphBox child)
        {
            child.SetLocalPosition(Zero, Height);
            child.AddGlobalPosition(GlobalPositionX + child.LocalPositionX, GlobalPositionY + child.LocalPositionY);

            Height += child.Height;
            if (child.Width > Width)
            {
                Width = child.Width;
            }
        }

        public override void CenterAlignChildren()
        {
            var positions = GetCenterAlignLocalPositions();

            for (var i = 0; i < _childContainers.Count; i++)
            {
                var child = _childContainers[i];
                if (child.SkipCentering) continue;
                child.AddGlobalPosition(positions[i], Zero);
                child.CenterAlignChildren();
            }
        }

        private List<float> GetCenterAlignLocalPositions()
        {
            var list = new List<float>();

            foreach (var child in _childContainers)
            {
                var gap = Width - child.Width;
                var shift = gap.Half();

                list.Add(shift);
            }

            return list;
        }
    }
}
