namespace Adnc.FluidBT.Tasks {
    public interface ITask {
        /// <summary>
        /// Is this task enabled or not? Disabled tasks are excluded from the runtime
        /// </summary>
        bool Enabled { get; set; }
        
        /// <summary>
        /// Check if this node is a valid lower priority abort
        /// </summary>
        bool IsLowerPriority { get; }
        
        /// <summary>
        /// Triggered every tick
        /// </summary>
        /// <returns></returns>
        TaskStatus Update ();

        /// <summary>
        /// Returns the valid abort condition
        /// </summary>
        /// <returns></returns>
        ITask GetAbortCondition ();

        /// <summary>
        /// Forcibly end this task. Firing all necessary completion logic
        /// </summary>
        void End ();

        /// <summary>
        /// Reset this task back to its initial state. Hard reset should run all
        /// one time run only logic (meant for pooling)
        /// </summary>
        /// <param name="hardReset"></param>
        void Reset (bool hardReset = false);
    }
}
