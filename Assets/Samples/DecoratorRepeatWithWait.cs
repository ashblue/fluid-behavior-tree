using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;

namespace CleverCrow.Fluid.BTs.Samples {
    public class DecoratorRepeatWithWait : MonoBehaviour {
        [SerializeField]
        private BehaviorTree _tree;

        [Tooltip("Setting to success will cause the task to succeed")]
        [SerializeField]
        private bool _isTaskSuccess;

        void Start () {
            _tree = new BehaviorTreeBuilder(gameObject)
                .RepeatForever()
                    .Parallel()

                        .Sequence()
                            .Do(() => TaskStatus.Success)
                            .WaitTime()
                            .Do(() => TaskStatus.Success)
                            .WaitTime()
                        .End()

                        .Sequence("Repeat until success is checked")
                            .Do(() => TaskStatus.Success)
                            .RepeatUntilSuccess()
                                .Sequence()
                                    .WaitTime()
                                    .Do(() => _isTaskSuccess ? TaskStatus.Success : TaskStatus.Failure)
                                .End()
                            .End()
                        .End()

                    .End()
                .End()
            .Build();
        }

        void Update () {
            _tree.Tick();
        }
    }
}
