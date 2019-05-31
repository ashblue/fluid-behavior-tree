namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class ConditionBase : TaskBase {
        protected abstract bool OnUpdate ();

        protected override TaskStatus GetUpdate () {
            if (OnUpdate()) {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}