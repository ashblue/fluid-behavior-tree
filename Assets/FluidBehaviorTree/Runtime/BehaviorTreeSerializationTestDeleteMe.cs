using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace FluidBehaviorTree.Runtime {
    public class BehaviorTreeSerializationTestDeleteMe : MonoBehaviour {
        [SerializeField] 
        private BehaviorTree _tree;

        [SerializeField]
        private bool _condition;
    
        private void Awake () {
            _tree = new BehaviorTreeBuilder(gameObject)
                .Sequence()
                    .Condition("Custom Condition", () => true)
                    .Do("Custom Action A", () => TaskStatus.Success)
                    .Selector()
                        .Sequence("Nested Sequence")
                            .Condition("Custom Condition", () => _condition)
                            .Do("Custom Action", () => TaskStatus.Success)
                        .End()
                        .Sequence("Nested Sequence")
                            .Condition("Custom Condition", () => true)
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
        }

        private void Update () {
            // Update our tree every frame
            _tree.Tick();
        }
    }
}
