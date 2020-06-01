using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CleverCrow.Fluid.BTs.Samples {
    public class DecoratorRepeatWithWait : MonoBehaviour {
        [SerializeField]
        private BehaviorTree _tree;

        [SerializeField]
        private bool _toggle;

        void Start () {
            _tree = new BehaviorTreeBuilder(gameObject)
                .RepeatForever()
                    .Parallel()

                        .Sequence()
                            .Do(() => {
                                _toggle = true;
                                return TaskStatus.Success;
                            })
                            .WaitTime()
                            .Do(() => {
                                _toggle = false;
                                return TaskStatus.Success;
                            })
                            .WaitTime()
                        .End()

                        .Sequence()
                            .Do(() => TaskStatus.Success)
                            .RepeatUntilSuccess()
                                .Sequence()
                                    .WaitTime()
                                    .Do(() => Random.value > 0.5f ? TaskStatus.Success : TaskStatus.Failure)
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
