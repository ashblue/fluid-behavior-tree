namespace Adnc.FluidBT.Tasks {
    public interface ITask {
        bool Enabled { get; set; }
        
        TaskStatus Update ();

        void Reset (bool hardReset = false);
    }
}
