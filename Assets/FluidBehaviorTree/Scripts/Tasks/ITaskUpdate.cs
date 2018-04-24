namespace Adnc.FluidBT.Tasks {
    public interface ITaskUpdate {
        bool Enabled { get; set; }
        TaskStatus Update ();
    }
}