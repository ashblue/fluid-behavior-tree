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

        private BehaviorTreeBuilder ParentTask<T> (string name) where T : ITaskParent, new() {
            var parent = new T { Name = name };
            _tree.AddNode(Pointer, parent);
            _pointer.Add(parent);

            return this;
        }
        
        public BehaviorTreeBuilder Sequence (string name = "sequence") {
            return ParentTask<Sequence>(name);
        }

        public BehaviorTreeBuilder Selector (string name = "selector") {
            return ParentTask<Selector>(name);
        }
        
        public BehaviorTreeBuilder Parallel (string name = "parallel") {
            return ParentTask<Parallel>(name);
        }

        public BehaviorTreeBuilder Do (string name, Func<TaskStatus> action) {
            _tree.AddNode(Pointer, new ActionGeneric {
                Name = name,
                updateLogic = action
            });
            
            return this;
        }
        
        public BehaviorTreeBuilder Do (Func<TaskStatus> action) {
            return Do("action", action);
        }

        public BehaviorTreeBuilder Condition (string name, Func<bool> action) {
            _tree.AddNode(Pointer, new ConditionGeneric {
                Name = name,
                updateLogic = action
            });

            return this;
        }
        
        public BehaviorTreeBuilder Condition (Func<bool> action) {
            return Condition("condition", action);
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