using System;
using System.Collections.Generic;
using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.TaskParents.Composites;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Tasks.Actions;
using UnityEngine;

namespace Adnc.FluidBT.Trees {
    public class BehaviorTreeBuilder {
        private readonly BehaviorTree _tree;
        private readonly List<ITaskParent> _pointer = new List<ITaskParent>();

        private ITaskParent Pointer {
            get {
                if (_pointer.Count == 0) return null;
                return _pointer[_pointer.Count - 1];
            }
        }
        
        public BehaviorTreeBuilder (GameObject owner) {
            _tree = new BehaviorTree(owner);
            _pointer.Add(_tree.Root);
        }
        
        public BehaviorTreeBuilder Sequence (string name) {
            var sequence = new Sequence { Name = name };
            _tree.AddNode(Pointer, sequence);
            _pointer.Add(sequence);

            return this;
        }

        public BehaviorTreeBuilder Do (string name, Func<TaskStatus> action) {
            _tree.AddNode(Pointer, new ActionGeneric {
                Name = name,
                updateLogic = action
            });
            
            return this;
        }

        public BehaviorTreeBuilder End () {
            _pointer.RemoveAt(_pointer.Count - 1);
            
            return this;
        }

        public BehaviorTree Build () {
            return _tree;
        }
    }
}