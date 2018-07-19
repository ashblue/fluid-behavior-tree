using Adnc.FluidBT.Trees;
using UnityEngine;
using Tree = Adnc.FluidBT.Trees.Tree;

namespace Adnc.FluidBT.Tasks {
    public interface ITask {
        /// <summary>
        /// Is this task enabled or not? Disabled tasks are excluded from the runtime
        /// </summary>
        bool Enabled { get; set; }
        
        /// <summary>
        /// Reference to the behavior tree responsible for this node. Allows for dynamic variables such as adding a
        /// GameObject reference
        /// </summary>
        GameObject Owner { get; set; }
        
        /// <summary>
        /// Tree this node belongs to
        /// </summary>
        Tree ParentTree { get; set; }

        /// <summary>
        /// Last status returned by Update
        /// </summary>
        TaskStatus LastStatus { get; }

        /// <summary>
        /// Triggered every tick
        /// </summary>
        /// <returns></returns>
        TaskStatus Update ();

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
