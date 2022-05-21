using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Trees;
#if UNITY_2021_3_OR_NEWER
using UnityEngine;
#endif

namespace CleverCrow.Fluid.BTs.Tasks
{
    public interface ITask
    {
        /// <summary>
        /// Used for debugging and identification purposes
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Is this task enabled or not? Disabled tasks are excluded from the runtime
        /// </summary>
        bool IsEnabled { get; set; }

        string IconPath { get; }

#if UNITY_2021_3_OR_NEWER
        /// <summary>
        /// Reference to the behavior tree responsible for this node. Allows for dynamic variables such as adding a
        /// GameObject reference
        /// </summary>
        GameObject Owner { get; set; }
#endif

        /// <summary>
        /// Tree this node belongs to
        /// </summary>
        IBehaviorTree ParentTree { get; set; }

        List<ITask> Children { get; }

        /// <summary>
        /// Last status returned by Update
        /// </summary>
        TaskStatus LastStatus { get; }

#if UNITY_EDITOR
        EditorRuntimeUtilities EditorUtils { get; }
#endif
        float IconPadding { get; }
        bool HasBeenActive { get; }

        /// <summary>
        /// Triggered every tick
        /// </summary>
        TaskStatus Update();

        /// <summary>
        /// Forcibly end this task. Firing all necessary completion logic
        /// </summary>
        void End();

        /// <summary>
        /// Reset this task back to its initial state to run again. Triggered after the behavior <br/>
        /// tree finishes with a task status other than continue.
        /// </summary>
        void Reset();
    }
}
