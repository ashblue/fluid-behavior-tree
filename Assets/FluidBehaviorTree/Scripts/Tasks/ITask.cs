namespace Adnc.FluidBT.Tasks {
    public interface ITask {
        bool Enabled { get; set; }
        
        TaskStatus Update ();

        ITask Tick ();

        void Reset (bool hardReset = false);
    }
}
