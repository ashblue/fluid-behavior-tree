namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class ConditionBase : TaskBase {
        public override string IconPath { get; } = $"{ICON_TASK_PATH}/Question.png";
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