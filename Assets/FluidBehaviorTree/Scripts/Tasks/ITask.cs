namespace Adnc.FluidBT.Tasks {
    public interface ITask {
        bool Enabled { get; set; }
        
        TaskStatus Update ();

        /**
         * Forcibly end this task. Firing all necessary completion logic
         */
        void End ();

        void Reset (bool hardReset = false);
    }
}
