namespace Adnc.FluidBT.TaskParents.Composites {
    public abstract class CompositeBase : TaskParentBase {
        public int ChildIndex { get; protected set; }

        public override void End () {
            if (ChildIndex < Children.Count) {
                Children[ChildIndex].End();
            }
        }
        
        public override void Reset () {
            ChildIndex = 0;

            base.Reset();
        }
    }
}