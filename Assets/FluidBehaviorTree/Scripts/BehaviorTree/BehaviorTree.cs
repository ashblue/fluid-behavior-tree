using System;
using System.Collections.Generic;
using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.Trees {
    public class BehaviorTree {
        private bool _setup;

        public ITaskRoot Root { get; } = new TaskRoot();

        public ITaskUpdate Current { get; set; }

        public readonly HashSet<object> nodes = new HashSet<object>();

        public readonly List<IEventAwake> nodeAwake = new List<IEventAwake>();

        public BehaviorTree () {
            Current = Root;
            nodes.Add(Root);
        }

        public void AddNode (ITaskChild parent, ITaskUpdate child) {
            if (parent == null) {
                throw new ArgumentNullException(nameof(parent));
            }

            if (parent.Child != null) {
                throw new ArgumentException("Cannot overwrite an already set child node");
            }

            if (child == null) {
                throw new ArgumentNullException(nameof(child));
            }

            if (!nodes.Contains(parent)) {
                throw new ArgumentException("Cannot add a node to a parent that is not in the BT");

            }

            if (nodes.Contains(child)) {
                throw new ArgumentException("Cannot set a child node that has already been added");
            }

            parent.Child = child;
            nodes.Add(child);

            var item = child as IEventAwake;
            if (item != null) {
                nodeAwake.Add(item);
            }
        }

        public void Setup () {
            if (!_setup) {
                _setup = true;
            } else {
                return;
            }

            nodeAwake.ForEach((n) => {
                n.Awake();
            });
        }

        public void Tick () {
            // Update must return the currently active node that should be reticked
            // @TODO Two different Update methods
                // Tick: Returns the next node to be ticked
                // Update: A status returned by a node (used by nodes that detect ticking)
            Current.Update();
        }
    }
}