using System;
using System.Collections.Generic;
using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.Trees {
    public class BehaviorTree {
        private bool _setup;

        public TaskRoot Root { get; } = new TaskRoot();

        public ITask Current { get; set; }

        public readonly HashSet<object> nodes = new HashSet<object>();

        public readonly List<IEventAwake> nodeAwake = new List<IEventAwake>();

        public BehaviorTree () {
            Current = Root;
            nodes.Add(Root);
        }

        public void AddNode (ITaskParent parent, ITask child) {
            if (parent == null) {
                throw new ArgumentNullException(nameof(parent));
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

            parent.AddChild(child);
            nodes.Add(child);
            child.Owner = this;

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

        public void Update () {
            Current.Update();
        }
    }
}