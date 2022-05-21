using System;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Decorators;
using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.TaskParents.Composites;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Trees
{
    // TODO: Create proper factory pattern with interface guiding
    public class BehaviorTreeBuilder
    {
        private const string DecoratorName = "decorator";
        private const string InverterName = "inverter";
        private const string ReturnSuccessName = "return success";
        private const string ReturnFailureName = "return failure";
        private const string RepeatUntilSuccessName = "repeat until success";
        private const string RepeatUntilFailureName = "repeat until failure";
        private const string RepeatForeverName = "repeat forever";
        private const string SequenceName = "sequence";
        private const string SelectorName = "selector";
        private const string SelectorRandomName = "selector random";
        private const string ParallelName = "parallel";
        private const string ActionName = "action";
        private const string WaitTimeName = "Wait Time";
        private const string ConditionName = "condition";
        private const string RandomChanceName = "random chance";
        private const string WaitName = "wait";

        private const float DefaultWaitTime = 1f;
        private const int DefaultRandomSeed = 0;
        private const int DefaultWaitTurns = 1;

        private ITaskParent PointerCurrent => _pointers.Count == 0 ? null : _pointers[^1];

        private readonly List<ITaskParent> _pointers = new(); //TODO: Starting size can reduce allocations
        private readonly BehaviorTree _tree;

        public BehaviorTreeBuilder(GameObject owner)
        {
            _tree = new BehaviorTree(owner);
            _pointers.Add(_tree.Root);
        }

        /// <summary>
        /// Set a name for the tree manually. For debugging purposes and visualizer window naming
        /// </summary>
        public BehaviorTreeBuilder Name(string name)
        {
            _tree.Name = name;
            return this;
        }

        public BehaviorTreeBuilder ParentTask<TTaskParent>(string name) where TTaskParent : ITaskParent, new()
        {
            var parent = new TTaskParent { Name = name };

            return AddNodeWithPointer(parent);
        }

        public BehaviorTreeBuilder Decorator(string name, Func<ITask, TaskStatus> logic)
        {
            var decorator = new DecoratorGeneric
            {
                UpdateLogic = logic,
                Name = name
            };

            return AddNodeWithPointer(decorator);
        }

        public BehaviorTreeBuilder AddNodeWithPointer(ITaskParent task)
        {
            AddNode(task);
            _pointers.Add(task);

            return this;
        }

        public BehaviorTreeBuilder Decorator(Func<ITask, TaskStatus> logic)
        {
            return Decorator(DecoratorName, logic);
        }

        public BehaviorTreeBuilder Inverter(string name = InverterName)
        {
            return ParentTask<Inverter>(name);
        }

        public BehaviorTreeBuilder ReturnSuccess(string name = ReturnSuccessName)
        {
            return ParentTask<ReturnSuccess>(name);
        }

        public BehaviorTreeBuilder ReturnFailure(string name = ReturnFailureName)
        {
            return ParentTask<ReturnFailure>(name);
        }

        public BehaviorTreeBuilder RepeatUntilSuccess(string name = RepeatUntilSuccessName)
        {
            return ParentTask<RepeatUntilSuccess>(name);
        }

        public BehaviorTreeBuilder RepeatUntilFailure(string name = RepeatUntilFailureName)
        {
            return ParentTask<RepeatUntilFailure>(name);
        }

        public BehaviorTreeBuilder RepeatForever(string name = RepeatForeverName)
        {
            return ParentTask<RepeatForever>(name);
        }

        public BehaviorTreeBuilder Sequence(string name = SequenceName)
        {
            return ParentTask<Sequence>(name);
        }

        public BehaviorTreeBuilder Selector(string name = SelectorName)
        {
            return ParentTask<Selector>(name);
        }

        /// <summary>
        /// Selects the first node to return success
        /// </summary>
        public BehaviorTreeBuilder SelectorRandom(string name = SelectorRandomName)
        {
            return ParentTask<SelectorRandom>(name);
        }

        public BehaviorTreeBuilder Parallel(string name = ParallelName)
        {
            return ParentTask<Parallel>(name);
        }

        public BehaviorTreeBuilder Do(string name, Func<TaskStatus> action)
        {
            return AddNode(new ActionGeneric
            {
                Name = name,
                UpdateLogic = action
            });
        }

        public BehaviorTreeBuilder Do(Func<TaskStatus> action)
        {
            return Do(ActionName, action);
        }

        /// <summary>
        /// Return continue until time has passed
        /// </summary>
        public BehaviorTreeBuilder WaitTime(string name, float time = 1f)
        {
            return AddNode(new WaitTime(new TimeMonitor())
            {
                Name = name,
                Time = time
            });
        }

        /// <summary>
        /// Return continue until time has passed
        /// </summary>
        public BehaviorTreeBuilder WaitTime(float time = DefaultWaitTime)
        {
            return WaitTime(WaitTimeName, time);
        }

        public BehaviorTreeBuilder Condition(string name, Func<bool> action)
        {
            return AddNode(new ConditionGeneric
            {
                Name = name,
                UpdateLogic = action
            });
        }

        public BehaviorTreeBuilder Condition(Func<bool> action)
        {
            return Condition(ConditionName, action);
        }

        public BehaviorTreeBuilder RandomChance(string name, int chance, int outOf, int seed = DefaultRandomSeed)
        {
            return AddNode(new RandomChance
            {
                Name = name,
                Chance = chance,
                OutOf = outOf,
                Seed = seed
            });
        }

        public BehaviorTreeBuilder RandomChance(int chance, int outOf, int seed = 0)
        {
            return RandomChance(RandomChanceName, chance, outOf, seed);
        }

        public BehaviorTreeBuilder Wait(string name, int turns = 1)
        {
            return AddNode(new Wait
            {
                Name = name,
                Turns = turns
            });
        }

        public BehaviorTreeBuilder Wait(int turns = DefaultWaitTurns)
        {
            return Wait(WaitName, turns);
        }

        public BehaviorTreeBuilder AddNode(ITask node)
        {
            _tree.AddNode(PointerCurrent, node);
            return this;
        }

        public BehaviorTreeBuilder Splice(BehaviorTree tree)
        {
            _tree.Splice(PointerCurrent, tree);

            return this;
        }

        public BehaviorTreeBuilder End()
        {
            _pointers.RemoveAt(_pointers.Count - 1);

            return this;
        }

        public BehaviorTree Build()
        {
            return _tree;
        }
    }
}
