namespace Adnc.FluidBT.TaskParents.Composites {
    public abstract class CompositeBase : TaskParentBase {
        public int ChildIndex { get; protected set; }

        public override void End () {
            if (ChildIndex < children.Count) {
                children[ChildIndex].End();
            }
        }
        
        public override void Reset (bool hardReset = false) {
            ChildIndex = 0;

            base.Reset(hardReset);
        }
    }
}