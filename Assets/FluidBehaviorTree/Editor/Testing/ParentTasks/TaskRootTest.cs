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

            [Test]
            public void It_should_add_a_child () {
                root.AddChild(Substitute.For<ITask>());

                Assert.AreEqual(1, root.children.Count);
            }

            [Test]
            public void It_should_not_allow_two_children () {
                root.AddChild(Substitute.For<ITask>());
                root.AddChild(Substitute.For<ITask>());

                Assert.AreEqual(1, root.children.Count);
            }
        }

        public class UpdateMethod : TaskRootTest {
            private ITask task;

            [SetUp]
            public void SetUpTask () {
                task = Substitute.For<ITask>();
                task.Enabled.Returns(true);

                root.AddChild(task);
            }

            [Test]
            public void Calls_task_update () {
                root.Update();

                task.Received().Update();
            }

            [Test]
            public void Returns_status_of_the_child () {
                task.Update().Returns(TaskStatus.Failure);

                Assert.AreEqual(TaskStatus.Failure, root.Update());
            }

            [Test]
            public void Return_success_if_no_child () {
                root.children.Clear();

                Assert.AreEqual(TaskStatus.Success, root.Update());
            }

            [Test]
            public void Does_not_tick_a_disabled_child_node () {
                task.Enabled.Returns(false);
                task.Update().Returns(TaskStatus.Success);

                root.Update();

                task.DidNotReceive().Update();
            }

            [Test]
            public void Does_tick_an_enabled_child_node () {
                root.Update();

                task.Received().Update();
            }
        }
    }
}
