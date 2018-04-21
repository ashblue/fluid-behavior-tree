using FluidBehaviorTree.Scripts.Nodes;
using NUnit.Framework;
using NSubstitute;

namespace Adnc.FluidBT.Testing {
    public class NodeRootTest {
        private NodeRoot root;

        [SetUp]
        public void SetUp () {
            root = new NodeRoot();
        }

        public class OnInit : NodeRootTest {
            [Test]
            public void It_should_initialize () {
                Assert.IsNotNull(root);
            }
        }

        public class UpdateMethod : NodeRootTest {
            private INodeUpdate action;

            [SetUp]
            public void SetUpUpdateMethod () {
                action = Substitute.For<INodeUpdate>();
                action.Enabled.Returns(true);

                root = new NodeRoot { Child = action };
            }

            [Test]
            public void Call_update_on_a_single_node () {
                root.Update();

                action.Received().Update();
            }

            [Test]
            public void Return_status_of_child () {
                action.Update().Returns(NodeStatus.Success);

                Assert.AreEqual(NodeStatus.Success, root.Update());
            }

            [Test]
            public void Return_status_failure_if_no_child () {
                root.Child = null;

                Assert.AreEqual(NodeStatus.Failure, root.Update());
            }

            [Test]
            public void Does_not_tick_a_disabled_child_node () {
                action.Enabled.Returns(false);
                action.Update().Returns(NodeStatus.Success);

                Assert.AreEqual(NodeStatus.Failure, root.Update());
            }

            [Test]
            public void Does_tick_an_enabled_child_node () {
                root.Update();

                action.Received().Update();
            }
        }
    }
}
