using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace FluidBehaviorTree.Runtime {
    public class BehaviorTreeSerializationTestDeleteMe : MonoBehaviour {
        [SerializeField] 
        private BehaviorTree _tree;
    
        private void Awake () {
            _tree = new BehaviorTreeBuilder(gameObject)
                .Sequence()
                    .Condition("Custom Condition", () => true)
                    .Do("Custom Action", () => TaskStatus.Success)
                    .Sequence("Nested Sequence")
                        .Condition("Custom Condition", () => true)
                        .Do("Custom Action", () => TaskStatus.Success)
                    .End()
                .End()
                .Build();
        }

        private void Update () {
            // Update our tree every frame
            _tree.Tick();
        }
    }
}
