using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Tasks {
    public abstract class ITask : ScriptableObject {
        /// <summary>
        /// Used for debugging and identification purposes
        /// </summary>
        public virtual string Name { get; set; }

        public Vector2 position = new Vector2(0.0f, 0.0f);
        public string guid;

        public void SetPosition(Vector2 pos)
        {
            position = pos;
        }

        /// <summary>
        /// Is this task enabled or not? Disabled tasks are excluded from the runtime
        /// </summary>
        public virtual bool Enabled { get; set; }
        
        public virtual string IconPath { get; }

        /// <summary>
        /// Reference to the behavior tree responsible for this node. Allows for dynamic variables such as adding a
        /// GameObject reference
        /// </summary>
        public GameObject Owner { get; set; }
        
        /// <summary>
        /// Tree this node belongs to
        /// </summary>
        public IBehaviorTree ParentTree { get; set; }
        
        public virtual List<ITask> Children { get; set; } = new List<ITask>();

        /// <summary>
        /// Last status returned by Update
        /// </summary>
        public TaskStatus LastStatus;
        
        public virtual EditorRuntimeUtilities EditorUtils { get; }
        public virtual float IconPadding { get; }
        public virtual bool HasBeenActive { get; set; }

        /// <summary>
        /// Triggered every tick
        /// </summary>
        /// <returns></returns>
        public virtual TaskStatus Update () { return TaskStatus.Success; }

        /// <summary>
        /// Forcibly end this task. Firing all necessary completion logic
        /// </summary>
        public virtual void End () {}

        /// <summary>
        /// Reset this task back to its initial state to run again. Triggered after the behavior
        /// tree finishes with a task status other than continue.
        /// </summary>
        public virtual void ResetTask () {}
    }
}
