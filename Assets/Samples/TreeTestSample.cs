using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Samples {
    /// <summary>
    /// Example script to test out BehaviorTrees, not actually compiled into the released package
    /// </summary>
    public class TreeTestSample : MonoBehaviour {
        [SerializeField]
        private BehaviorTree _treeA;

        [SerializeField]
        private BehaviorTree _treeB;

        [SerializeField]
        private BehaviorTree _treeC;

        [SerializeField]
        private BehaviorTree _treeD;

        [SerializeField]
        private bool _condition = false;

        private void Awake () {
            _treeA = new BehaviorTreeBuilder(gameObject)
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

            _treeB = new BehaviorTreeBuilder(gameObject)
                .Name("Basic")
                .Sequence()
                    .Condition("Custom Condition", () => _condition)
                    .Do("Custom Action A", () => TaskStatus.Success)
                .End()
                .Build();

            _treeC = new BehaviorTreeBuilder(gameObject)
                .Name("Basic")
                .Sequence()
                    .Condition("Custom Condition", () => _condition)
                    .Do("Continue", () => _condition ? TaskStatus.Continue : TaskStatus.Success)
                .End()
                .Build();

            _treeD = new BehaviorTreeBuilder(gameObject)
                .Sequence()
                    .TrueSelectorRandom()
                        .Do("Action A", () => TaskStatus.Success)
                        .Do("Action B", () => TaskStatus.Success)
                    .End()
                    .WaitTime(2.0f)
                .End()
                .Build();
        }

        private void Update () {
            // Update our tree every frame
            _treeA.Tick();
            _treeB.Tick();
            _treeC.Tick();
            _treeD.Tick();
        }
    }
}
