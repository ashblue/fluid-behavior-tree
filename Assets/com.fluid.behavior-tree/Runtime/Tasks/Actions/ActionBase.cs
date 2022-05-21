namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    public abstract class ActionBase : TaskBase
    {

        protected override TaskStatus GetUpdate() => OnUpdate();
        protected abstract TaskStatus OnUpdate();
    }
}
