namespace CleverCrow.Fluid.BTs.Tasks.Actions
{
    public interface ITimeMonitor
    {
        float DeltaTime { get; }

        void OnTick();
        void Reset();
    }
}
