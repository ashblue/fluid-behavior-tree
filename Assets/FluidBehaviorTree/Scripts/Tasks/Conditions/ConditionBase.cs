namespace Adnc.FluidBT.Tasks {
    public abstract class ConditionBase : TaskBase {
        public override bool ValidAbortCondition { get; } = true;
        protected abstract bool OnUpdate ();

        protected override TaskStatus GetUpdate () {
            if (OnUpdate()) {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}