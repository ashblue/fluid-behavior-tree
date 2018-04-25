using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using NUnit.Framework;
using NSubstitute;

namespace Adnc.FluidBT.Testing {
    public class TaskRootTest {
        private TaskRoot root;

        [SetUp]
        public void SetUp () {
            root = new TaskRoot();
        }

        public class OnInit : TaskRootTest {
            [Test]
            public void It_should_initialize () {
                Assert.IsNotNull(root);
            }
        }

        public class TickMethod : TaskRootTest {
            private ITask action;

            [SetUp]
            public void SetUpUpdateMethod () {
                action = Substitute.For<ITask>();
                action.Enabled.Returns(true);

                root = new TaskRoot();
                root.AddChild(action);
            }

            [Test]
            public void Call_update_on_a_single_node () {
                root.Tick();

                action.Received().Update();
            }

            [Test]
            public void Returns_null_on_child_success () {
                action.Update().Returns(TaskStatus.Success);

                Assert.AreEqual(null, root.Tick());
            }

            [Test]
            public void Return_null_if_no_child () {
                root.children.Clear();

                Assert.AreEqual(null, root.Tick());
            }

            [Test]
            public void Does_not_tick_a_disabled_child_node () {
                action.Enabled.Returns(false);
                action.Update().Returns(TaskStatus.Success);

                root.Tick();

                action.DidNotReceive().Update();
            }

            [Test]
            public void Does_tick_an_enabled_child_node () {
                root.Tick();

                action.Received().Update();
            }
        }
    }
}
