using FluidBehaviorTree.Scripts.Nodes;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class NodeActionTest {
        public class NodeActionExample : NodeAction {
            public int StartCount { get; private set; }
            public NodeStatus status = NodeStatus.Success;

            public override NodeStatus OnUpdate () {
                return status;
            }

            public override void OnStart () {
                StartCount++;
            }
        }

        public class UpdateMethod {
            NodeActionExample node;

            [SetUp]
            public void SetUpNode () {
                node = new NodeActionExample();
            }

            public class StartEvent : UpdateMethod {
                [Test]
                public void Trigger_the_start_method () {
                    node.Update();

                    Assert.AreEqual(1, node.StartCount);
                }

                [Test]
                public void Retrigger_the_start_method_when_success_is_returned () {
                    node.Update();
                    node.Update();

                    Assert.AreEqual(2, node.StartCount);
                }

                [Test]
                public void Rerigger_the_start_method_when_failure_is_returned () {
                    node.status = NodeStatus.Failure;

                    node.Update();
                    node.Update();

                    Assert.AreEqual(2, node.StartCount);
                }

                [Test]
                public void Do_not_retrigger_the_start_method_when_continue_is_returned () {
                    node.status = NodeStatus.Continue;

                    node.Update();
                    node.Update();

                    Assert.AreEqual(1, node.StartCount);
                }

                [Test]
                public void Trigger_the_start_method_after_continue_passes () {
                    node.status = NodeStatus.Continue;

                    node.Update();
                    node.status = NodeStatus.Success;
                    node.Update();
                    node.Update();

                    Assert.AreEqual(2, node.StartCount);
                }
            }
        }
    }
}