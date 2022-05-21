using UnityEngine;
using static CleverCrow.Fluid.BTs.Trees.Utils.MathFExtensions;

namespace CleverCrow.Fluid.BTs.Trees.Editors
{
    public class BehaviorTreePrinter
    {
        private const float ScrollPadding = 40;

        public static StatusIcons StatusIcons { get; private set; }
        public static GuiStyleCollection SharedStyles { get; private set; }

        private readonly VisualTask _root;
        private readonly Rect _containerSize;

        private Vector2 _scrollPosition;

        public BehaviorTreePrinter(IBehaviorTree tree, Vector2 windowSize)
        {
            StatusIcons = new StatusIcons();
            SharedStyles = new GuiStyleCollection();

            var container = new GraphContainerVertical();
            container.SetGlobalPosition(ScrollPadding, ScrollPadding);
            _root = new VisualTask(tree.Root, container);
            container.CenterAlignChildren();

            _containerSize = new Rect(Zero, Zero,
                container.Width + ScrollPadding.Twice(),
                container.Height + ScrollPadding.Twice());

            CenterScrollView(windowSize, container);
        }

        private void CenterScrollView(Vector2 windowSize, GraphContainerVertical container)
        {
            var scrollOverflow = container.Width + ScrollPadding.Twice() - windowSize.x;
            var centerViewPosition = scrollOverflow.Half();
            _scrollPosition.x = centerViewPosition;
        }

        public void Print(Vector2 windowSize)
        {
            _scrollPosition = GUI.BeginScrollView(
                new Rect(Zero, Zero, windowSize.x, windowSize.y),
                _scrollPosition,
                _containerSize
            );
            _root.Print();
            GUI.EndScrollView();
        }

        public void Unbind()
        {
            _root.RecursiveTaskUnbind();
        }
    }
}
