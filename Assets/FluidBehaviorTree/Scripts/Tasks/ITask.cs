namespace Adnc.FluidBT.Tasks {
    public interface ITask {
        bool Enabled { get; set; }
        bool IsLowerPriority { get; }

        TaskStatus Update ();

        ITask GetAbortCondition ();

        /**
         * Forcibly end this task. Firing all necessary completion logic
         */
        void End ();

        void Reset (bool hardReset = false);
    }
}
