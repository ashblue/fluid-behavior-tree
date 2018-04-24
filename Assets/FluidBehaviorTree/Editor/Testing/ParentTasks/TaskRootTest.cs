using Adnc.FluidBT.TaskParents;
using Adnc.FluidBT.Tasks;
using NUnit.Framework;
using NSubstitute;

namespace Adnc.FluidBT.Testing {
    public class TaskRootTest {
        private ITaskRoot root;

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

        public class UpdateMethod : TaskRootTest {
            private ITask action;

            [SetUp]
            public void SetUpUpdateMethod () {
                action = Substitute.For<ITask>();
                action.Enabled.Returns(true);

                root = new TaskRoot { Child = action };
            }

            [Test]
            public void Call_update_on_a_single_node () {
                root.Update();

                action.Received().Update();
            }

            [Test]
            public void Return_status_of_child () {
                action.Update().Returns(TaskStatus.Success);

                Assert.AreEqual(TaskStatus.Success, root.Update());
            }

            [Test]
            public void Return_status_failure_if_no_child () {
                root.Child = null;

                Assert.AreEqual(TaskStatus.Failure, root.Update());
            }

            [Test]
            public void Does_not_tick_a_disabled_child_node () {
                action.Enabled.Returns(false);
                action.Update().Returns(TaskStatus.Success);

                Assert.AreEqual(TaskStatus.Failure, root.Update());
            }

            [Test]
            public void Does_tick_an_enabled_child_node () {
                root.Update();

                action.Received().Update();
            }
        }
    }
}
