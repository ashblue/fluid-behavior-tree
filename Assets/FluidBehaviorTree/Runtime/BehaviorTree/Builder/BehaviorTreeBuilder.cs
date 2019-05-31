using System;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees {
    public class BehaviorTreeBuilder {
        private readonly BehaviorTree _tree;
        private readonly List<ITaskParent> _pointers = new List<ITaskParent>();

        private ITaskParent PointerCurrent {
            get {
                if (_pointers.Count == 0) return null;
                return _pointers[_pointers.Count - 1];
            }
        }

        public BehaviorTreeBuilder (GameObject owner) {
            _tree = new BehaviorTree(owner);
            _pointers.Add(_tree.Root);
        }

        public BehaviorTreeBuilder ParentTask<P> (string name) where P : ITaskParent, new() {
            var parent = new P { Name = name };

            return AddNodeWithPointer(parent);
        }

        public BehaviorTreeBuilder Decorator (string name, Func<ITask, TaskStatus> logic) {
            var decorator = new DecoratorGeneric {
                updateLogic = logic,
                Name = name
            };

            return AddNodeWithPointer(decorator);
        }

        public BehaviorTreeBuilder AddNodeWithPointer (ITaskParent task) {
            AddNode(task);
            _pointers.Add(task);
            
            return this;
        }
        
        public BehaviorTreeBuilder Decorator (Func<ITask, TaskStatus> logic) {
            return Decorator("decorator", logic);
        }

        public BehaviorTreeBuilder Inverter (string name = "inverter") {
            return ParentTask<Inverter>(name);
        }
        
        public BehaviorTreeBuilder ReturnSuccess (string name = "return success") {
            return ParentTask<ReturnSuccess>(name);
        }
        
        public BehaviorTreeBuilder ReturnFailure (string name = "return failure") {
            return ParentTask<ReturnFailure>(name);
        }
        
        public BehaviorTreeBuilder Sequence (string name = "sequence") {
            return ParentTask<Sequence>(name);
        }

        public BehaviorTreeBuilder Selector (string name = "selector") {
            return ParentTask<Selector>(name);
        }
        
        /// <summary>
        /// Selects the first node to return success
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BehaviorTreeBuilder SelectorRandom (string name = "selector random") {
            return ParentTask<SelectorRandom>(name);
        }
        
        public BehaviorTreeBuilder Parallel (string name = "parallel") {
            return ParentTask<Parallel>(name);
        }

        public BehaviorTreeBuilder Do (string name, Func<TaskStatus> action) {
            return AddNode(new ActionGeneric {
                Name = name,
                updateLogic = action
            });
        }
        
        public BehaviorTreeBuilder Do (Func<TaskStatus> action) {
            return Do("action", action);
        }

        public BehaviorTreeBuilder Condition (string name, Func<bool> action) {
            return AddNode(new ConditionGeneric {
                Name = name,
                updateLogic = action
            });
        }
        
        public BehaviorTreeBuilder Condition (Func<bool> action) {
            return Condition("condition", action);
        }
        
        public BehaviorTreeBuilder RandomChance (string name, int chance, int outOf, int seed = 0) {
            return AddNode(new RandomChance {
                Name = name,
                chance = chance,
                outOf = outOf,
                seed = seed
            });
        }

        public BehaviorTreeBuilder RandomChance (int chance, int outOf, int seed = 0) {
            return RandomChance("random chance", chance, outOf, seed);
        }

        public BehaviorTreeBuilder Wait (string name, int turns = 1) {
            return AddNode(new Wait {
                Name = name,
                turns = turns
            });
        }

        public BehaviorTreeBuilder Wait (int turns = 1) {
            return Wait("wait", turns);
        }
        
        public BehaviorTreeBuilder AddNode (ITask node) {
            _tree.AddNode(PointerCurrent, node);
            return this;
        }

        public BehaviorTreeBuilder Splice (BehaviorTree tree) {
            _tree.Splice(PointerCurrent, tree);

            return this;
        }

        public BehaviorTreeBuilder End () {
            _pointers.RemoveAt(_pointers.Count - 1);
            
            return this;
        }

        public BehaviorTree Build () {
            return _tree;
        }
    }
}