namespace CleverCrow.Fluid.BTs.Trees.Editors {
    public class GraphContainerVertical : GraphContainerHorizontal {
        public override void AddBox (IGraphBox child) {
            CalculateChild(child);
            _childContainers.Add(child);
        }

        private void CalculateChild (IGraphBox child) {
            child.SetLocalPosition(0, Height);
            child.AddGlobalPosition(GlobalPositionX + child.LocalPositionX, GlobalPositionY + child.LocalPositionY);

            Height += child.Height;
            if (child.Width > Width) Width = child.Width;
        }
    }
}