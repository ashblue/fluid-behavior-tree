using FluidBehaviorTree.Scripts.Nodes;
using NUnit.Framework;

namespace Adnc.FluidBT.Testing {
    public class NodeActionTest {
        public class NodeActionExample : NodeAction {
            public int StartCount { get; private set; }
            public int InitCount { get; private set; }
            public NodeStatus status = NodeStatus.Success;

            protected override NodeStatus OnUpdate () {
                return status;
            }

            protected override void OnStart () {
                StartCount++;
            }

            protected override void OnInit () {
                InitCount++;
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
                public void Trigger_on_first_run () {
                    node.Update();

                    Assert.AreEqual(1, node.StartCount);
                }

                [Test]
                public void Retrigger_when_success_is_returned () {
                    node.Update();
                    node.Update();

                    Assert.AreEqual(2, node.StartCount);
                }

                [Test]
                public void Rerigger_when_failure_is_returned () {
                    node.status = NodeStatus.Failure;

                    node.Update();
                    node.Update();

                    Assert.AreEqual(2, node.StartCount);
                }

                [Test]
                public void Does_not_retrigger_when_continue_is_returned () {
                    node.status = NodeStatus.Continue;

                    node.Update();
                    node.Update();

                    Assert.AreEqual(1, node.StartCount);
                }

                [Test]
                public void Triggers_after_continue_passes () {
                    node.status = NodeStatus.Continue;

                    node.Update();
                    node.status = NodeStatus.Success;
                    node.Update();
                    node.Update();

                    Assert.AreEqual(2, node.StartCount);
                }

                [Test]
                public void Triggers_on_reset () {
                    node.status = NodeStatus.Continue;

                    node.Update();
                    node.Reset();
                    node.Update();
                    
                    Assert.AreEqual(2, node.StartCount);
                }
            }

            public class InitEvent : UpdateMethod {
                [Test]
                public void Triggers_on_1st_update () {
                    node.Update();

                    Assert.AreEqual(1, node.InitCount);
                }
                
                [Test]
                public void Does_not_trigger_on_2nd_update () {
                    node.Update();
                    node.Update();

                    Assert.AreEqual(1, node.InitCount);
                }

                [Test]
                public void Does_not_trigger_on_reset () {
                    node.Update();
                    node.Reset();
                    node.Update();

                    Assert.AreEqual(1, node.InitCount);
                }

                [Test]
                public void Triggers_on_reset_hard () {
                    node.Update();
                    node.Reset(true);
                    node.Update();

                    Assert.AreEqual(2, node.InitCount);
                }
            }
        }
    }
}