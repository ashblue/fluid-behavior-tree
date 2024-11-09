namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class ConditionBase : TaskBase {
        public override string IconPath { get; } = $"{PACKAGE_ROOT}/Question.png";
        public override float IconPadding => 10;
        
        protected abstract bool OnUpdate ();

        protected override TaskStatus GetUpdate () {
            if (OnUpdate()) {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}