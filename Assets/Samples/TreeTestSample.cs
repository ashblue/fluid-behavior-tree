using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using CleverCrow.Fluid.BTs.Trees.Core.Interfaces;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Samples
{
    /// <summary>
    /// Example script to test out BehaviorTrees, not actually compiled into the released package
    /// </summary>
    public class TreeTestSample : MonoBehaviour, IOwner
    {
        public string Name => name;

        [SerializeField] private BehaviorTree _treeA;

        [SerializeField] private BehaviorTree _treeB;

        [SerializeField] private BehaviorTree _treeC;

        [SerializeField] private bool _condition = false;

        private void Awake()
        {
            _treeA = new BehaviorTreeBuilder(this)
                .Sequence()
                .Condition("Custom Condition", () => true)
                .Do("Custom Action A", () => TaskStatus.Success)
                .Selector()
                .Sequence("Nested Sequence")
                .Condition("Custom Condition", () => _condition)
                .Do("Custom Action", () => TaskStatus.Success)
                .End()
                .Sequence("Nested Sequence")
                .Do("Custom Action", () => TaskStatus.Success)
                .Sequence("Nested Sequence")
                .Condition("Custom Condition", () => true)
                .Do("Custom Action", () => TaskStatus.Success)
                .End()
                .End()
                .Do("Custom Action", () => TaskStatus.Success)
                .Condition("Custom Condition", () => true)
                .End()
                .Do("Custom Action B", () => TaskStatus.Success)
                .End()
                .Build();

            _treeB = new BehaviorTreeBuilder(this)
                .Name("Basic")
                .Sequence()
                .Condition("Custom Condition", () => _condition)
                .Do("Custom Action A", () => TaskStatus.Success)
                .End()
                .Build();

            _treeC = new BehaviorTreeBuilder(this)
                .Name("Basic")
                .Sequence()
                .Condition("Custom Condition", () => _condition)
                .Do("Continue", () => _condition ? TaskStatus.Continue : TaskStatus.Success)
                .End()
                .Build();
        }

        private void Update()
        {
            // Update our tree every frame
            _treeA.Tick();
            _treeB.Tick();
            _treeC.Tick();
        }
    }
}
