using System;
using FluidBehaviorTree.Scripts.Nodes;
using System.Collections.Generic;

namespace Adnc.FluidBT.Trees {
    public class BehaviorTree {
        public INodeRoot Root { get; } = new NodeRoot();

        public INodeUpdate Current { get; set; }

        public readonly HashSet<object> nodes = new HashSet<object>();

        // @TODO Should only contain awake nodes
        public readonly List<object> nodeList = new List<object>();

        public BehaviorTree () {
            Current = Root;
            nodes.Add(Root);
        }

        public void AddNode (INodeChild parent, INodeUpdate child) {
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
            nodeList.Add(child);
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