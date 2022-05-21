using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class NodeFaders
    {
        private static readonly Color DefaultBackgroundStartColor = new(0.65f, 0.65f, 0.65f);
        private static readonly Color DefaultBackgroundEndColor = new(0.39f, 0.78f, 0.39f);
        private static readonly Color DefaultIconStartColor = new(1, 1, 1, 0.3f);
        private static readonly Color DefaultIconEndColor = new(1, 1, 1, 1f);

        public ColorFader BackgroundFader { get; } = new(DefaultBackgroundStartColor, DefaultBackgroundEndColor);

        public ColorFader TextFader { get; } = new(Color.white, Color.black);

        public ColorFader MainIconFader { get; } = new(DefaultIconStartColor, DefaultIconEndColor);

        public void Update(bool isActive)
        {
            BackgroundFader.Update(isActive);
            TextFader.Update(isActive);
            MainIconFader.Update(isActive);
        }
    }
}
