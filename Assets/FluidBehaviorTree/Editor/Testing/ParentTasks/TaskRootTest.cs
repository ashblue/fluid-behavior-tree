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

        public class TickMethod {
            public class WithParentTaskAsChild : TaskRootTest {
                private ITaskParent taskParent;

                [SetUp]
                public void SetUpTask () {
                    taskParent = Substitute.For<ITaskParent>();
                    taskParent.Enabled.Returns(true);

                    root.AddChild(taskParent);
                }

                [Test]
                public void Ticks_parent_task () {
                    taskParent.Tick();

                    taskParent.Received().Tick();
                }

                [Test]
                public void Tick_returns_parent_task_status () {
                    taskParent.Tick().Returns(taskParent);

                    Assert.AreEqual(taskParent, root.Tick());
                }

                [Test]
                public void Does_not_run_update_on_child () {
                    root.Tick();

                    taskParent.DidNotReceive().Update();
                }
            }

            public class WithTaskAsChild : TaskRootTest {
                private ITask task;

                [SetUp]
                public void SetUpTask () {
                    task = Substitute.For<ITask>();
                    task.Enabled.Returns(true);

                    root.AddChild(task);
                }

                [Test]
                public void Does_not_run_tick_on_child () {
                    root.Tick();

                    task.DidNotReceive().Tick();
                }

                [Test]
                public void Calls_task_update () {
                    root.Tick();

                    task.Received().Update();
                }

                [Test]
                public void Returns_null_on_task_success () {
                    task.Update().Returns(TaskStatus.Success);

                    Assert.AreEqual(null, root.Tick());
                }

                [Test]
                public void Returns_null_on_task_failure () {
                    task.Update().Returns(TaskStatus.Failure);

                    Assert.AreEqual(null, root.Tick());
                }

                [Test]
                public void Returns_self_on_task_continue () {
                    task.Update().Returns(TaskStatus.Continue);

                    Assert.AreEqual(root, root.Tick());
                }

                [Test]
                public void Reruns_node_after_task_continue () {
                    task.Update().Returns(TaskStatus.Continue);

                    root.Tick();
                    root.Tick();

                    task.Received(2).Update();
                }

                [Test]
                public void Return_null_if_no_child () {
                    root.children.Clear();

                    Assert.AreEqual(null, root.Tick());
                }

                [Test]
                public void Does_not_tick_a_disabled_child_node () {
                    task.Enabled.Returns(false);
                    task.Update().Returns(TaskStatus.Success);

                    root.Tick();

                    task.DidNotReceive().Update();
                }

                [Test]
                public void Does_tick_an_enabled_child_node () {
                    root.Tick();

                    task.Received().Update();
                }
            }
        }
    }
}
