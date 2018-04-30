namespace Adnc.FluidBT.Tasks.Actions {
    public abstract class ActionBase : TaskBase {
        public override bool ValidAbortCondition { get; } = false;
        protected abstract TaskStatus OnUpdate ();

        protected override TaskStatus GetUpdate () {
            return OnUpdate();
        }
    }
}