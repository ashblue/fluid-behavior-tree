using System;
using System.Collections.Generic;
using Adnc.FluidBT.Decorators;
using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.TaskParents.Composites;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Tasks.Actions;
using UnityEngine;

namespace Adnc.FluidBT.Trees {
    /// <summary>
    /// This class can be extended with your own custom BehaviorTreeBuilder code
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BehaviorTreeBuilderBase<T> where T : BehaviorTreeBuilderBase<T> {
        protected readonly BehaviorTree _tree;
        protected readonly List<ITaskParent> _pointer = new List<ITaskParent>();

        protected ITaskParent Pointer {
            get {
                if (_pointer.Count == 0) return null;
                return _pointer[_pointer.Count - 1];
            }
        }

        protected BehaviorTreeBuilderBase (GameObject owner) {
            _tree = new BehaviorTree(owner);
            _pointer.Add(_tree.Root);
        }

        protected T ParentTask<P> (string name) where P : ITaskParent, new() {
            var parent = new P { Name = name };
            _tree.AddNode(Pointer, parent);
            _pointer.Add(parent);

            return (T)this;
        }

        public T Decorator (string name, Func<ITask, TaskStatus> logic) {
            var decorator = new DecoratorGeneric {
                updateLogic = logic,
                Name = name
            };
                        
            _tree.AddNode(Pointer, decorator);
            _pointer.Add(decorator);
            
            return (T)this;
        }
        
        public T Decorator (Func<ITask, TaskStatus> logic) {
            return Decorator("decorator", logic);
        }

        public T Inverter (string name = "inverter") {
            return ParentTask<Inverter>(name);
        }
        
        public T ReturnSuccess (string name = "return success") {
            return ParentTask<ReturnSuccess>(name);
        }
        
        public T ReturnFailure (string name = "return failure") {
            return ParentTask<ReturnFailure>(name);
        }
        
        public T Sequence (string name = "sequence") {
            return ParentTask<Sequence>(name);
        }

        public T Selector (string name = "selector") {
            return ParentTask<Selector>(name);
        }
        
        public T Parallel (string name = "parallel") {
            return ParentTask<Parallel>(name);
        }

        public T Do (string name, Func<TaskStatus> action) {
            _tree.AddNode(Pointer, new ActionGeneric {
                Name = name,
                updateLogic = action
            });
            
            return (T)this;
        }
        
        public T Do (Func<TaskStatus> action) {
            return Do("action", action);
        }

        public T Condition (string name, Func<bool> action) {
            _tree.AddNode(Pointer, new ConditionGeneric {
                Name = name,
                updateLogic = action
            });

            return (T)this;
        }
        
        public T Condition (Func<bool> action) {
            return Condition("condition", action);
        }
        
        public T RandomChance (string name, int chance, int outOf) {
            _tree.AddNode(Pointer, new RandomChance {
                Name = name,
                chance = chance,
                outOf = outOf
            });

            return (T)this;
        }

        public T RandomChance (int chance, int outOf) {
            return RandomChance("random chance", chance, outOf);
        }

        public T Wait (string name, int turns = 1) {
            _tree.AddNode(Pointer, new Wait {
                Name = name,
                turns = turns
            });

            return (T)this;
        }

        public T Wait (int turns = 1) {
            return Wait("wait", turns);
        }

        public T End () {
            _pointer.RemoveAt(_pointer.Count - 1);
            
            return (T)this;
        }

        public BehaviorTree Build () {
            return _tree;
        }
    }
}