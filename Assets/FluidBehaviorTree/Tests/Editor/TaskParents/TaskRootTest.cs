using CleverCrow.Fluid.BTs.TaskParents;
using CleverCrow.Fluid.BTs.Tasks;
using NUnit.Framework;
using NSubstitute;

namespace CleverCrow.Fluid.BTs.Testing {
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
                root.AddChild(A.TaskStub().Build());

                Assert.AreEqual(1, root.Children.Count);
            }

            [Test]
            public void It_should_not_allow_two_children () {
                root.AddChild(A.TaskStub().Build());
                root.AddChild(A.TaskStub().Build());

                Assert.AreEqual(1, root.Children.Count);
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
                root.Children.Clear();

                Assert.AreEqual(TaskStatus.Success, root.Update());
            }

            [Test]
            public void Does_tick_an_enabled_child_node () {
                root.Update();

                task.Received().Update();
            }
        }
    }
}
